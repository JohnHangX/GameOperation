﻿//
// ICECreatureMarkerEditor.cs
// Version 1.4.0
//
// Copyrights © Pit Vetterick, ICE Technologies Consulting LTD. All Rights Reserved.
// http://www.icecreaturecontrol.com
// mailto:support@icecreaturecontrol.com
// 
// Unity Asset Store End User License Agreement (EULA)
// http://unity3d.com/legal/as_terms
//
// ##############################################################################

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEditor.AnimatedValues;

using ICE.World.EditorUtilities;

using ICE.Creatures;
using ICE.Creatures.Utilities;
using ICE.Creatures.Objects;
using ICE.Creatures.EnumTypes;
using ICE.Creatures.Attributes;

using ICE.Creatures.EditorInfos;
using ICE.Creatures.EditorUtilities;

namespace ICE.Creatures
{
	[CustomEditor(typeof(ICECreatureShelter))]
	public class ICECreatureShelterEditor : ICECreatureLocationEditor {
		
		public override void OnInspectorGUI()
		{
			ICECreatureShelter _target = DrawEntityHeader<ICECreatureShelter>();
			DrawShelterContent( _target );
			DrawFooter( _target );

		}

		public virtual void DrawShelterContent( ICECreatureShelter _target )
		{
			if( _target == null )
				return;

		}
	}
}