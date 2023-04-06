﻿using System;
using poetools.Core;
using UnityEngine;

namespace poetools.Console
{
    // todo: add a scriptableObject search + create editor window! 
    // that would help w/ one-off SO like this that need to be made.
    
    [CreateAssetMenu(menuName = RuntimeConsoleNaming.AssetMenuName + "/Default Prefix")]
    public class DefaultLogPrefix : LogPrefix
    {
        [SerializeField] 
        private Color color = Color.red;

        public override string GenerateMessage(string category)
        {
            string formattedCategory = category.ToLower().Color(color);
            string formattedTime = DateTime.Now.ToShortTimeString().Bold();

            return $"[{formattedCategory}@{formattedTime}]  ";  
        }
    }
}