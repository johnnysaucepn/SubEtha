using System;
using System.Collections.Generic;
using System.Reactive.Concurrency;
using System.Reactive.Linq;

namespace Howatworks.Thumb.Console
{
    public class ConsoleKeyListener
    {
        public Lazy<IObservable<ConsoleKeyInfo>> _observable = new Lazy<IObservable<ConsoleKeyInfo>>(() => GetObservable());

        public IObservable<ConsoleKeyInfo> Observable => _observable.Value;

        private static IEnumerable<ConsoleKeyInfo> KeyPresses()
        {
            ConsoleKeyInfo key;
            do
            {
                key = System.Console.ReadKey(true);
                yield return key;
            } while (true);
        }

        private static IObservable<ConsoleKeyInfo> GetObservable()
        {
            return KeyPresses().ToObservable(TaskPoolScheduler.Default).Publish().RefCount();
        }

    }

}
