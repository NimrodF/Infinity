using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectCircularBuffer
{
    class ProducerConsumer4
    {
        private static int itemsToProduce = 30;
        private static int itemsToConsume = 30;
        private const int NumOfConsumers = 8;
        private const int NumOfProducers = 2;
        private const uint bufferSize = 5;
        private static CircularBuffer<uint> products = new CircularBuffer<uint>(bufferSize);
        private static uint counter = 1;
        private static List<uint> output = new List<uint>(itemsToProduce);

        static void Main(string[] args)
        {
            for (int i = 0; i < NumOfConsumers; ++i)
            {
                Task.Run(new Action(Consume));
            }

            for (int i = 0; i < NumOfProducers; ++i)
            {
                Task.Run(new Action(Produce));
            }

            Console.WriteLine();

            foreach (uint curr in output)
            {
                Console.Write(curr + ", ");
                Console.Out.Flush();
            }


            Console.ReadLine();
        }

        private static void Produce()
        {
            while (itemsToProduce > 0)
            {
                Console.WriteLine("Produce: {0}", counter);
                products.Write(counter++);
                --itemsToProduce;
            }

        }

        private static void Consume()
        {
            while (itemsToConsume > 0)
            {
                output.Add(products.Read());
                Console.WriteLine("Consume: {0}", products.Read());
                --itemsToConsume;
            }
        }
    }
}
