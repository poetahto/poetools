using System.Collections.Generic;
using UnityEngine;

namespace poetools.Core
{
    public class TagHolder : MonoBehaviour
    {
        public List<Tag> tags;

        public bool HasAny(params Tag[] desiredTags)
        {
            foreach (var desiredTag in desiredTags)
            {
                foreach (var tag1 in tags)
                {
                    if (tag1 == desiredTag)
                        return true;
                }
            }

            return false;
        }

        public bool HasAll(params Tag[] desiredTags)
        {
            int remaining = desiredTags.Length;

            foreach (var desiredTag in desiredTags)
            {
                foreach (var tag1 in tags)
                {
                    if (tag1 == desiredTag)
                        remaining--;
                }
            }

            return remaining <= 0;
        }

        public void Add(params Tag[] desiredTags)
        {
            tags.AddRange(desiredTags);
        }
    }

    public static class TagExtensions
    {
        public static void AddTags(this GameObject target, params Tag[] tags)
        {
            var tagHolder = EnsureHasTagHolder(target);
            tagHolder.Add(tags);
        }

        public static bool HasAnyTag(this GameObject target, params Tag[] tags)
        {
            if (target.TryGetComponent(out TagHolder tagHolder))
                return tagHolder.HasAny(tags);

            return false;
        }

        public static bool HasAllTags(this GameObject target, params Tag[] tags)
        {
            if (target.TryGetComponent(out TagHolder tagHolder))
                return tagHolder.HasAll(tags);

            return false;
        }

        private static TagHolder EnsureHasTagHolder(GameObject target)
        {
            if (!target.TryGetComponent(out TagHolder tagHolder))
                tagHolder = target.AddComponent<TagHolder>();

            return tagHolder;
        }
    }
}
