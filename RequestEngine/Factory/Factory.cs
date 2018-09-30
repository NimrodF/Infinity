using System;
using System.Collections.Generic;

namespace RequestEngine
{
    public class Factory<TKey, TBase, TParam>
    {
        private static readonly Dictionary<TKey, Func<TParam, TBase>> products = new Dictionary<TKey, Func<TParam, TBase>>();

        public Factory() { }

        public bool Add(TKey key, Func<TParam, TBase> creator)
        {
            if (creator == null)
            {
                throw new NullCreatorException();
            }

            bool overriden = false;

            if (products.ContainsKey(key))
            {
                overriden = true;
            }

            products.Add(key, creator);

            return overriden;
        }

        public TBase Create(TKey key, TParam param)
        {
            TBase product;

            if (products.TryGetValue(key, out Func<TParam, TBase> creator))
            {
                try
                {
                    product = creator(param);
                }
                catch (Exception exc)
                {
                    throw new CreateFailException(exc.Message);
                }
            }
            else
            {
                throw new NoKeyException();
            }

            return product;
        }

        public void Remove(TKey key)
        {
            if (!products.Remove(key))
            {
                throw new NoKeyException();
            }
        }
    }
}

