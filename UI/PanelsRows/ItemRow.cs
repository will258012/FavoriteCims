﻿using AlgernonCommons.Translation;
using AlgernonCommons.UI;
using ColossalFramework;
using ColossalFramework.Globalization;
using ColossalFramework.UI;
using FavoriteCims.Utils;
using UnityEngine;

namespace FavoriteCims.UI.PanelsRows
{
    public class ItemRow : UIListRow
    {
        private InstanceID citizenInstanceID;

        private Citizen citizen;

        private uint citizenId;

        private int realAge;

        private bool isTourist = false;

        private UIButton gender;

        private UIButton _name;

        private UIButton age;

        private UIButton star;

        public override void Awake()
        {
            base.Awake();
            width = 226f;
            height = 25f;
            name = "ItemRow";
            atlas = MyAtlas.FavCimsAtlas;
            BackgroundSpriteName = "bg_row1";
            SelectedSpriteName = "bg_row1";
            backgroundSprite = "bg_row1";
            relativePosition = new Vector3(0f, 0f);
            gender = AddUIComponent<UIButton>();
            gender.name = "ItemGender";
            gender.width = 17f;
            gender.height = 17f;
            gender.atlas = MyAtlas.FavCimsAtlas;
            gender.relativePosition = new Vector3(5f, 4f);
            _name = AddUIComponent<UIButton>();
            _name.name = "ItemName";
            _name.width = 131f;
            _name.height = 25f;
            _name.textVerticalAlignment = UIVerticalAlignment.Middle;
            _name.textHorizontalAlignment = 0;
            _name.playAudioEvents = true;
            _name.font = UIFonts.Regular;
            _name.font.size = 15;
            _name.textScale = 0.8f;
            _name.useDropShadow = true;
            _name.dropShadowOffset = new Vector2(1f, -1f);
            _name.dropShadowColor = new Color32(0, 0, 0, 0);
            _name.textPadding.left = 5;
            _name.textPadding.right = 5;
            _name.textColor = new Color32(204, 204, 51, 40);
            _name.hoveredTextColor = new Color32(204, 102, 0, 20);
            _name.pressedTextColor = new Color32(102, 153, byte.MaxValue, 147);
            _name.focusedTextColor = new Color32(153, 0, 0, 0);
            _name.disabledTextColor = new Color32(51, 51, 51, 160);
            _name.relativePosition = new Vector3(gender.relativePosition.x + gender.width, 1f);
            _name.eventMouseUp += delegate (UIComponent component, UIMouseEventParameter eventParam)
            {
                FavCimsCore.GoToCitizen(position, citizenInstanceID, isTourist, eventParam);
            };
            age = AddUIComponent<UIButton>();
            age.name = "ItemAge";
            age.width = 23f;
            age.height = 19f;
            age.textHorizontalAlignment = UIHorizontalAlignment.Center;
            age.textVerticalAlignment = UIVerticalAlignment.Middle;
            age.font = UIFonts.Regular;
            age.textScale = 0.9f;
            age.font.size = 15;
            age.dropShadowOffset = new Vector2(1f, -1f);
            age.dropShadowColor = new Color32(0, 0, 0, 0);
            age.isInteractive = false;
            age.relativePosition = new Vector3(_name.relativePosition.x + _name.width + 10f, 4f);
            star = AddUIComponent<UIButton>();
            star.name = "ItemStar";
            star.atlas = MyAtlas.FavCimsAtlas;
            star.size = new Vector2(16f, 16f);
            star.playAudioEvents = true;
            star.relativePosition = new Vector3(age.relativePosition.x + age.width + 10f, 4f);
            star.eventClick += delegate (UIComponent component, UIMouseEventParameter eventParam)
            {
                FavCimsCore.AddToFavorites(citizenInstanceID);
                if (FavCimsCore.RowID.Contains((int)citizenId))
                {
                    (component as UIButton).normalBgSprite = "icon_fav_subscribed";
                    (component as UIButton).tooltip = Translations.Translate("FavStarButton_disable_tooltip");
                }
                else
                {
                    (component as UIButton).normalBgSprite = "icon_fav_unsubscribed";
                    component.tooltip = Translations.Translate("FavStarButton_enable_tooltip");
                }
            };
        }

        public override void Display(object data, int rowIndex)
        {
            citizenId = (uint)data;
            citizen = CitizenManager.instance.m_citizens.m_buffer[citizenId];
            citizenInstanceID.Citizen = citizenId;
            CitizenInfo citizenInfo = citizen.GetCitizenInfo(citizenId);
            VehicleInfo vehicleInfo = null;
            BuildingInfo buildingInfo = null;
            
            string localizedStatus = citizenInfo.m_citizenAI.GetLocalizedStatus(citizenId, ref citizen, out InstanceID target);
            string buildingName = BuildingManager.instance.GetBuildingName(target.Building, citizenInstanceID);

            if (citizen.m_vehicle != 0)
            {
                vehicleInfo = Singleton<VehicleManager>.instance.m_vehicles.m_buffer[citizen.m_vehicle].Info;
            }
            if (target.Building != 0)
            {
                buildingInfo = Singleton<BuildingManager>.instance.m_buildings.m_buffer[target.Building].Info;
            }

            if (citizenInfo.m_class.m_service == ItemClass.Service.Tourism)
            {
                isTourist = true;
                gender.tooltip = Locale.Get("CITIZEN_OCCUPATION_TOURIST");
                _name.tooltip = localizedStatus + " " + buildingName;
                if (Citizen.GetGender(citizenId) == Citizen.Gender.Female)
                {
                    _name.textColor = new Color32(byte.MaxValue, 102, 204, 213);
                    gender.normalBgSprite = "touristIcon";
                }
                else
                {
                    _name.textColor = new Color32(204, 204, 51, 40);
                    gender.normalBgSprite = "touristIcon";
                }
            }
            else
            {
                gender.tooltip = Locale.Get("ASSETTYPE_CITIZEN");
                if (Citizen.GetGender(citizenId) == Citizen.Gender.Female)
                {
                    _name.textColor = new Color32(byte.MaxValue, 102, 204, 213);
                    gender.normalBgSprite = "Female";
                }
                else
                {
                    _name.textColor = new Color32(204, 204, 51, 40);
                    gender.normalBgSprite = "Male";
                }
                _name.tooltip = citizen.Arrested ? Translations.Translate("Jailed_into") + " " + buildingName : localizedStatus + " " + buildingName;
            }

            if (citizen.GetBuildingByLocation() == target.Building)
            {

                gender.normalFgSprite = "greenArrowIcon";

            }
            else
            {
                if (buildingInfo && buildingInfo.m_class.m_service == ItemClass.Service.Residential)
                {
                    gender.normalFgSprite = "redArrowIcon";
                }
            }

            if (vehicleInfo && vehicleInfo.m_class.m_service == ItemClass.Service.PublicTransport)
            {
                gender.normalFgSprite = "greenArrowIcon";
            }

            _name.text = CitizenManager.instance.GetCitizenName(citizenId);
            realAge = FavCimsCore.CalculateCitizenAge(citizen.m_age);

            switch (realAge)
            {
                case int n when n <= 12:
                    age.text = realAge.ToString();
                    age.textColor = new Color32(83, 166, 0, 60);
                    break;
                case int n when n <= 19:
                    age.text = realAge.ToString();
                    age.textColor = new Color32(0, 102, 51, 100);
                    break;
                case int n when n <= 25:
                    age.text = realAge.ToString();
                    age.textColor = new Color32(byte.MaxValue, 204, 0, 32);
                    break;
                case int n when n <= 65:
                    age.text = realAge.ToString();
                    age.textColor = new Color32(byte.MaxValue, 102, 0, 16);
                    break;
                case int n when n <= 90:
                    age.text = realAge.ToString();
                    age.textColor = new Color32(153, 0, 0, 0);
                    break;
                default:
                    age.text = realAge.ToString();
                    age.textColor = new Color32(byte.MaxValue, 0, 0, 0);
                    break;
            }
            if (FavCimsCore.RowID.Contains((int)citizenId))
            {
                star.normalBgSprite = "icon_fav_subscribed";
                star.tooltip = Translations.Translate("FavStarButton_disable_tooltip");
            }
            else
            {
                star.normalBgSprite = "icon_fav_unsubscribed";
                star.tooltip = Translations.Translate("FavStarButton_enable_tooltip");
            }

            Deselect(rowIndex);
        }

        public override void Deselect(int rowIndex)
        {
            //Always use lighter background
            BackgroundSpriteName = null;
        }
    }
}
