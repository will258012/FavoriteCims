using AlgernonCommons.Translation;
using ColossalFramework.UI;
using FavoriteCims.UI.Panels;
using FavoriteCims.Utils;
using UnityEngine;

namespace FavoriteCims.UI.Buttons
{
    public class PassengersInsideVehiclesButton : UIButton
    {
        private InstanceID VehicleID = InstanceID.Empty;

        public UIAlignAnchor Alignment;

        public UIPanel RefPanel;

        private PeopleInsideVehiclesPanel VehiclePanel;

        public override void Start()
        {
            UIView aview = UIView.GetAView();
            name = "FavCimsVehPassButton";
            normalBgSprite = "vehicleButton";
            hoveredBgSprite = "vehicleButtonHovered";
            focusedBgSprite = "vehicleButtonHovered";
            pressedBgSprite = "vehicleButtonHovered";
            disabledBgSprite = "vehicleButtonDisabled";
            atlas = MyAtlas.FavCimsAtlas;
            size = new Vector2(36f, 36f);
            playAudioEvents = true;
            AlignTo(RefPanel, Alignment);
            tooltipBox = aview.defaultTooltipBox;
            VehiclePanel = MainClass.FullScreenContainer.AddUIComponent(typeof(PeopleInsideVehiclesPanel)) as PeopleInsideVehiclesPanel;
            VehiclePanel.VehicleID = InstanceID.Empty;
            VehiclePanel.Hide();
            eventClick += delegate (UIComponent component, UIMouseEventParameter eventParam)
            {
                if (!VehicleID.IsEmpty && !VehiclePanel.isVisible)
                {
                    VehiclePanel.VehicleID = VehicleID;
                    VehiclePanel.RefPanel = RefPanel;
                    VehiclePanel.Show();
                }
                else
                {
                    VehiclePanel.VehicleID = InstanceID.Empty;
                    VehiclePanel.Hide();
                }
            };
        }

        public override void Update()
        {
            bool unLoading = MainClass.UnLoading;
            if (!unLoading)
            {
                bool isVisible = base.isVisible;
                if (isVisible)
                {
                    tooltip = Translations.Translate("View_NoPassengers");
                    if (WorldInfoPanel.GetCurrentInstanceID() != InstanceID.Empty)
                    {
                        VehicleID = WorldInfoPanel.GetCurrentInstanceID();
                    }
                    if (VehiclePanel != null)
                    {
                        if (!VehiclePanel.isVisible)
                        {
                            Unfocus();
                        }
                        else
                        {
                            Focus();
                        }
                    }
                    if (!VehicleID.IsEmpty && VehicleID.Type == InstanceType.Vehicle)
                    {
                        isEnabled = true;
                        tooltip = Translations.Translate("View_PassengersList");
                    }
                    else
                    {
                        VehiclePanel.Hide();
                        Unfocus();
                        isEnabled = false;
                    }
                }
                else
                {
                    isEnabled = false;
                    VehiclePanel.Hide();
                    VehicleID = InstanceID.Empty;
                }
            }
        }
    }
}
