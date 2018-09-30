using System;
using System.Collections.Generic;
using System.Threading;

namespace RequestEngine
{
    public class ThreadPool<T> where T : IExecutable
    {
        private const uint defaultNumberOfThreads = 6;
        private uint threadsCount = defaultNumberOfThreads;
        private readonly List<Thread> threads = new List<Thread>();
        private readonly PriorityQueue<TaskPair> tasksQueue = new PriorityQueue<TaskPair>();

        private class TaskPair : IExecutable, IComparable<TaskPair>
        {
            internal IExecutable task;
            internal Priority priority;

            internal TaskPair(IExecutable _task, Priority _priority)
            {
                task = _task;
                priority = _priority;
            }

            public int CompareTo(TaskPair other)
            {
                return other.priority - priority;
            }

            public void Execute()
            {
                task.Execute();
            }
        }

        public ThreadPool(uint _threadsCount)
        {
            threadsCount = _threadsCount;

            for (int i = 0; i < threadsCount; ++i)
            {
                MakeThread();
            }
        }

        public enum Priority
        {
            Low = 1,
            Medium,
            High
        }

        public void AddTask(T task, Priority taskPriority)
        {
            tasksQueue.Push(new TaskPair(task, taskPriority));
        }

        public void Stop(int millisecondsTimeout)
        {
            Thread.Sleep(millisecondsTimeout);

            for (int i = 0; i < threadsCount; ++i)
            {
                tasksQueue.Push(new TaskPair(new BadApple(), Priority.High));
            }
        }

        public uint NumberOfThreads
        {
            get
            {
                return threadsCount;
            }

            set
            {
                AdjustNumberOfThreads(value);
            }
        }

        private void AdjustNumberOfThreads(uint newNumberOfThreads)
        {
            if (newNumberOfThreads >= threadsCount)
            {
                uint threadsToCreate = newNumberOfThreads - threadsCount;

                for (uint i = 0; i < threadsToCreate; ++i)
                {
                    MakeThread();
                }
            }
            else
            {
                uint threadsToKill = threadsCount - newNumberOfThreads;

                for (uint i = 0; i < threadsToKill; ++i)
                {
                    tasksQueue.Push(new TaskPair(new BadApple(), Priority.High));
                }
            }
        }

        private void MakeThread()
        {
            Thread newThread = new Thread(ThreadMethod);
            threads.Add(newThread);
            newThread.Start();
        }

        private void ThreadMethod()
        {
            while (true)
            {
                try
                {
                    tasksQueue.Pop().task.Execute();
                }
                catch (Exception exc)
                {
                    Console.WriteLine("Thread exception:" + exc.Message);
                }
            }
        }
    }
}
