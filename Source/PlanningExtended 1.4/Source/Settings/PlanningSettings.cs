﻿using System.Collections.Generic;
using PlanningExtended.Defs;
using PlanningExtended.Designations;
using Verse;

namespace PlanningExtended.Settings
{
    public class PlanningSettings : ModSettings
    {
        public bool useUndoRedo = Default.UseUndoRedo;

        public int maxUndoOperations = Default.MaxUndoRedoSteps;

        public bool displayCutDesignator = Default.DisplayCutDesignator;

        public bool displayChangePlanAppearanceDesignator = Default.DisplayChangePlanAppearanceDesignator;

        public bool displayTogglePlanVisibilityDesignator = Default.DisplayTogglePlanVisibilityDesignator;

        public bool areDesignationsPersistent = Default.AreDesignationsPersistent;

        public bool useCtrlForColorDialog = Default.UseCtrlForColorDialog;

        public bool alwaysGrabBottom = Default.AlwaysGrabBottom;

        public Dictionary<PlanDesignationType, PlanDesignationSetting> planDesignationSettings = new();

        public override void ExposeData()
        {
            Scribe_Values.Look(ref useUndoRedo, nameof(useUndoRedo), Default.UseUndoRedo);
            Scribe_Values.Look(ref maxUndoOperations, nameof(maxUndoOperations), Default.MaxUndoRedoSteps);
            Scribe_Values.Look(ref displayCutDesignator, nameof(displayCutDesignator), Default.DisplayCutDesignator);
            Scribe_Values.Look(ref displayChangePlanAppearanceDesignator, nameof(displayChangePlanAppearanceDesignator), Default.DisplayChangePlanAppearanceDesignator);
            Scribe_Values.Look(ref displayTogglePlanVisibilityDesignator, nameof(displayTogglePlanVisibilityDesignator), Default.DisplayTogglePlanVisibilityDesignator);
            Scribe_Values.Look(ref areDesignationsPersistent, nameof(areDesignationsPersistent), Default.AreDesignationsPersistent);
            Scribe_Values.Look(ref useCtrlForColorDialog, nameof(useCtrlForColorDialog), Default.UseCtrlForColorDialog);
            //Scribe_Values.Look(ref alwaysGrabBottom, nameof(alwaysGrabBottom), false);

            Scribe_Collections.Look<PlanDesignationType, PlanDesignationSetting>(ref planDesignationSettings, nameof(planDesignationSettings), LookMode.Value, LookMode.Deep);

            if (Scribe.mode == LoadSaveMode.LoadingVars)
                InitData();

            base.ExposeData();
        }

        public void Reset()
        {
            useUndoRedo = Default.UseUndoRedo;
            maxUndoOperations = Default.MaxUndoRedoSteps;
            displayCutDesignator = Default.DisplayCutDesignator;
            areDesignationsPersistent = Default.AreDesignationsPersistent;
            useCtrlForColorDialog = Default.UseCtrlForColorDialog;
            alwaysGrabBottom = Default.AlwaysGrabBottom;
        }

        public void SetOpacity(PlanDesignationType planDesignationType, float opacity)
        {
            if (planDesignationType == PlanDesignationType.Unknown)
            {
                foreach (PlanDesignationSetting planDesignationSetting in planDesignationSettings.Values)
                    planDesignationSetting.opacity = opacity;
            }
            else
            {
                if (planDesignationSettings.TryGetValue(planDesignationType, out PlanDesignationSetting setting))
                    setting.opacity = opacity;
            }
        }

        public float GetOpacity(PlanDesignationType planDesignationType)
        {
            return (planDesignationSettings.TryGetValue(planDesignationType, out PlanDesignationSetting planDesignationSetting)) ? planDesignationSetting.opacity : 1f;
        }

        void InitData()
        {
            planDesignationSettings ??= new();

            foreach (PlanDesignationType planDesignationType in PlanDesignationUtilities.GetPlanDesignationTypes())
            {
                if (!planDesignationSettings.ContainsKey(planDesignationType))
                    planDesignationSettings[planDesignationType] = new PlanDesignationSetting(1f, "", PlanTextureSet.Dashed);
            }

            //foreach (DesignationDefContainer designationDefContainer in PlanningDesignationDefOf.DesignationDefs)
            //{
            //    if (!planDesignationSettings.ContainsKey(designationDefContainer.Type))
            //        //planDesignationSettings[designationDefContainer.Type] = new PlanDesignationSetting(1f, ColorDefinitions.NonColoredDef.defName, PlanTextureSet.Dashed);
            //        planDesignationSettings[designationDefContainer.Type] = new PlanDesignationSetting(1f, "", PlanTextureSet.Dashed);
            //}
        }

        class Default
        {
            public const bool UseUndoRedo = true;

            public const int MaxUndoRedoSteps = 20;

            public const bool DisplayCutDesignator = true;

            public const bool DisplayChangePlanAppearanceDesignator = false;

            public const bool DisplayTogglePlanVisibilityDesignator = false;

            public const bool AreDesignationsPersistent = true;

            public const bool AlwaysGrabBottom = false;

            public const bool UseCtrlForColorDialog = false;
        }
    }
}
