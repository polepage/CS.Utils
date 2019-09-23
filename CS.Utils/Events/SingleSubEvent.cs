using Prism.Events;
using System;

namespace CS.Utils.Events
{
    public class SingleSubEvent : PubSubEvent
    {
        protected override SubscriptionToken InternalSubscribe(IEventSubscription eventSubscription)
        {
            if (Subscriptions.Count > 0)
            {
                throw new InvalidOperationException("SingleSubEvent supports only one subscriber");
            }

            return base.InternalSubscribe(eventSubscription);
        }
    }

    public class SingleSubEvent<TPayload> : PubSubEvent<TPayload>
    {
        protected override SubscriptionToken InternalSubscribe(IEventSubscription eventSubscription)
        {
            if (Subscriptions.Count > 0)
            {
                throw new InvalidOperationException("SingleSubEvent<TPayload> supports only one subscriber");
            }

            return base.InternalSubscribe(eventSubscription);
        }
    }
}
