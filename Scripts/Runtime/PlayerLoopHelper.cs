using System;
using System.Collections.Generic;
using System.Text;
using JetBrains.Annotations;
using UnityEngine.LowLevel;
using UnityEngine.PlayerLoop;

[PublicAPI] 
public static class PlayerLoopHelper
{
    // Stores data about a disabled system
    private readonly struct SystemNode
    {
        public readonly PlayerLoopSystem System;
        public readonly int Index;
        public readonly Type Parent;

        public SystemNode(PlayerLoopSystem system, int index, Type parent)
        {
            System = system;
            Index = index;
            Parent = parent;
        }
    }
        
    // The node on which all subsystems branch from
    private static PlayerLoopSystem _defaultRoot = PlayerLoop.GetDefaultPlayerLoop();
    private static readonly List<SystemNode> DeactivatedSystems = new List<SystemNode>();
        
    /// <summary>
    /// Re-enables all systems that have been disabled through the PlayerLoopUtil
    /// </summary>
    public static void EnableAllSystems()
    {
        for (int i = 0; i < DeactivatedSystems.Count; i++)
        {
            var cur = DeactivatedSystems[i];
            InsertSystem(cur.Parent, cur.System, cur.Index + i);
        }
        DeactivatedSystems.Clear();
    }

    /// <summary>
    /// Disable the PlayerLoopSystem specified by T
    /// </summary>
    /// <param name="root">The root node to search for T</param>
    /// <typeparam name="T">The type of the PlayerLoopSystem to disable, e.g. <see cref="Update"/>, <see cref="FixedUpdate"/></typeparam>
    /// <returns>Whether or not this system was successfully disabled</returns>
    public static bool DisableSystem<T>(ref PlayerLoopSystem root)
    {
        if (root.subSystemList == null) return false;
        
        for (var i = 0; i < root.subSystemList.Length; i++)
        {
            if (root.subSystemList[i].type == typeof(T))
            {
                var node = new SystemNode(root.subSystemList[i], i, root.type );
                DeactivatedSystems.Add(node);
                Remove(i, ref root.subSystemList);
                return true;
            }

            DisableSystem<T>(ref root.subSystemList[i]);
        }
        return false;
    }
        
    public static bool DisableSystem<T>()
    {
        return DisableSystem<T>(ref _defaultRoot);
    }

    /// <summary>
    /// Searches a specified PlayerLoopSystem for the system of type T
    /// </summary>
    /// <param name="root">The node to search</param>
    /// <typeparam name="T">The type of the system that is being searched for, e.g. <see cref="Update"/>, <see cref="FixedUpdate"/></typeparam>
    /// <returns>The PlayerLoopSystem that was found in the root</returns>
    public static PlayerLoopSystem FindSystem<T>(PlayerLoopSystem root)
    {
        if (root.type == typeof(T)) return root;
        if (root.subSystemList != null)
        {
            foreach (var system in root.subSystemList)
            {
                var result = FindSystem<T>(system);
                if (result.type == typeof(T)) return result;
            }    
        }
        return default;
    }
        
    public static PlayerLoopSystem FindSystem<T>()
    {
        return FindSystem<T>(_defaultRoot);
    }
    
    /// <summary>
    /// Searches a PlayerLoopSystem for a system of type T, and replaces it with a different system
    /// </summary>
    /// <param name="replacement">The system to replace T</param>
    /// <param name="root">The PlayerLoopSystem to search for T</param>
    /// <typeparam name="T">The type of the system that should be replaced, e.g. <see cref="Update"/>, <see cref="FixedUpdate"/></typeparam>
    /// <returns>Whether or not we successfully replaced T with replacement</returns>
    public static bool ReplaceSystem<T>(PlayerLoopSystem replacement, ref PlayerLoopSystem root)
    {
        // check to see if the current system is the one we are looking for
        if (root.type == typeof(T))
        {
            root = replacement;
            return true;
        }
        
        // search this systems children to see if they have the one we are looking for
        if (root.subSystemList != null)
        {
            for (var i = 0; i < root.subSystemList.Length; i++)
            {
                if (ReplaceSystem<T>(replacement, ref root.subSystemList[i]))
                {
                    return true;
                }
            }
        }
        return false;
    }
        
    public static bool ReplaceSystem<T>(PlayerLoopSystem replacement)
    {
        return ReplaceSystem<T>(replacement, ref _defaultRoot);
    }
    
    /// <summary>
    /// Inserts a PlayerLoopSystem into another PlayerLoopSystem at a specified index, under a specified parent.
    /// </summary>
    /// <param name="insertion">The PlayerLoopSystem to be inserted into the root</param>
    /// <param name="root">The PlayerLoopSystem that will contain the new PlayerLoopSystem</param>
    /// <param name="index">The index that the new PlayerLoopSystem should have. The lowest index (0) means it will execute first</param>
    /// <typeparam name="T">The parent of the node that is being inserted</typeparam>
    /// <returns>Whether or not the node was inserted successfully</returns>
    public static bool InsertSystem<T>(PlayerLoopSystem insertion, ref PlayerLoopSystem root, int index = 0)
    {
        if (root.type == typeof(T) || typeof(T) == typeof(PlayerLoopSystem))
        {
            Insert(insertion, index, ref root.subSystemList);
            return true;
        }

        if (root.subSystemList != null)
        {
            for (int i = 0; i < root.subSystemList.Length; i++)
            {
                if (InsertSystem<T>(insertion, ref root.subSystemList[i]))
                {
                    return true;
                }
            }
        }

        return false;
    }
        
    public static bool InsertSystem<T>(PlayerLoopSystem insertion, int index = 0)
    {
        return InsertSystem<T>(insertion, ref _defaultRoot, index);
    }
        
    public static bool InsertSystem(Type parent, PlayerLoopSystem insertion, ref PlayerLoopSystem root, int index = 0)
    {
        if (root.type == parent || parent == typeof(PlayerLoopSystem))
        {
            Insert(insertion, index, ref root.subSystemList);
            return true;
        }

        if (root.subSystemList != null)
        {
            for (int i = 0; i < root.subSystemList.Length; i++)
            {
                if (InsertSystem(parent, insertion, ref root.subSystemList[i]))
                {
                    return true;
                }
            }
        }

        return false;
    }
        
    public static bool InsertSystem(Type parent, PlayerLoopSystem insertion, int index = 0)
    {
        return InsertSystem(parent, insertion, ref _defaultRoot, index);
    }

    /// <summary>
    /// Converts a PlayerLoopSystem into a readable string
    /// </summary>
    /// <param name="root">The PlayerLoopSystem to convert</param>
    /// <returns>A string listing every subsystem that is attached to the root</returns>
    public static string SystemToString(PlayerLoopSystem root)
    {
        var sb = new StringBuilder();
        PrintSystemTree(sb, root, 0);
        return sb.ToString();
    }
        
    public static string SystemToString()
    {
        return SystemToString(_defaultRoot);
    }

    // Private helper methods, performs array operations and generates a string representing a PlayerLoopSystem 
    private static void Insert<T>(T element, int index, ref T[] array)
    {
        var result = new T[array.Length + 1];
        
        result[index] = element;
        Array.ConstrainedCopy(array, 0, result, 0, index);
        Array.ConstrainedCopy(array, index, result, index + 1, array.Length - index);

        array = result;
    }
        
    private static void Remove<T>(int index, ref T[] array)
    {
        var result = new T[array.Length - 1];
        Array.ConstrainedCopy(array, 0, result, 0, index);
        Array.ConstrainedCopy(array, index + 1, result, index, array.Length - index - 1);

        array = result;
    }
        
    private static void PrintSystemTree(StringBuilder builder, PlayerLoopSystem root, int depth = 0)
    {
        // make sure head node is printed
        if (depth == 0) builder.AppendLine("HEAD");
        
        // print out the name of the current system (if it exists)
        if (root.type != null)
        {
            for (int i = 0; i < depth; i++)
            {
                builder.Append("\t");   
            }
            builder.AppendLine(root.type.Name);
        }

        // print out the names of this system's subsystems (if they exist)
        if (root.subSystemList != null)
        {
            depth++;
            foreach (var system in root.subSystemList)
            {
                PrintSystemTree(builder, system, depth);
            }
        }
    }
}