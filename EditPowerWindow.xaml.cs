using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using CharPad.Framework;

namespace CharPad
{
    /// <summary>
    /// Interaction logic for EditPowerWindow.xaml
    /// </summary>
    public partial class EditPowerWindow : Window
    {
        private List<BindableEnum<PowerType>> powerTypes;
        private List<BindableEnum<PowerActionType>> actionTypes;
        private List<BindableEnum<PowerAttackType>> attackTypes;
        private List<BindableEnum<WeaponSlot>> attackWeaponValues;
        private List<BindableEnum<AttributeType>> attackAttributeValues;
        private List<BindableEnum<DefenseType>> defenseTypeValues;
        private List<BindableEnum<AttributeType>> bonusDamageAttributeValues;

        private BasicAdjustmentList attackAdjustments;
        private BasicAdjustmentList damageAdjustments;

        public EditPowerWindow(Power power)
        {
            powerTypes = BindableEnum<PowerType>.BuildValues(new Dictionary<PowerType, string>
            {
                { PowerType.AtWill, "At-Will" }
            });

            actionTypes = BindableEnum<PowerActionType>.BuildValues(new Dictionary<PowerActionType, string>
            {
                { PowerActionType.ImmediateReaction, "Immediate Reaction" },
                { PowerActionType.ImmediateInterrupt, "Immediate Interrupt" }
            });

            attackTypes = BindableEnum<PowerAttackType>.BuildValues(null,
                new List<PowerAttackType> { PowerAttackType.None });

            attackWeaponValues = BindableEnum<WeaponSlot>.BuildValues(new Dictionary<WeaponSlot, string>
            {
                { WeaponSlot.MainWeapon, "Main Weapon" },
                { WeaponSlot.OffhandWeapon, "Off-hand Weapon" },
                { WeaponSlot.RangedWeapon, "Ranged Weapon" }
            });

            attackAttributeValues = BindableEnum<AttributeType>.BuildValues(null,
                new List<AttributeType> { AttributeType.Wildcard });

            defenseTypeValues = BindableEnum<DefenseType>.BuildValues(null);

            bonusDamageAttributeValues = BindableEnum<AttributeType>.BuildValues(null,
                new List<AttributeType> { AttributeType.Wildcard });

            InitializeComponent();

            txtName.Text = power.Name;
            txtLevel.Text = power.Level.ToString();
            txtDescription.Text = power.Description;
            cboPowerType.SelectedItem = powerTypes.Find(x => x.Value == power.PowerType);
            cboActionType.SelectedItem = actionTypes.Find(x => x.Value == power.ActionType);

            if (power.AttackType == PowerAttackType.None)
                chkIsActionPower.IsChecked = false;
            else
            {
                chkIsActionPower.IsChecked = true;

                cboAttackType.SelectedItem = attackTypes.Find(x => x.Value == power.AttackType);
                cboWeapon.SelectedItem = attackWeaponValues.Find(x => x.Value == power.AttackWeapon);
                cboAttackAttr.SelectedItem = 
            }

            attackAdjustments = new BasicAdjustmentList();

            foreach (BasicAdjustment adjustment in power.AttackModifiers)
            {
                attackAdjustments.Add(new BasicAdjustment(adjustment));
            }

            damageAdjustments = new BasicAdjustmentList();

            foreach (BasicAdjustment adjustment in power.DamageModifiers)
            {
                damageAdjustments.Add(new BasicAdjustment(adjustment));
            }
        }

        private List<BindableEnum<PowerType>> PowerTypes { get { return powerTypes; } }
        private List<BindableEnum<PowerActionType>> ActionTypes { get { return actionTypes; } }
        private List<BindableEnum<PowerAttackType>> AttackTypes { get { return attackTypes; } }
        private List<BindableEnum<WeaponSlot>> AttackWeaponValues { get { return attackWeaponValues; } }
        private List<BindableEnum<AttributeType>> AttackAttributeValues { get { return attackAttributeValues; } }
        private List<BindableEnum<DefenseType>> DefenseTypeValues { get { return defenseTypeValues; } }
        private List<BindableEnum<AttributeType>> BonusDamageAttributeValues { get { return bonusDamageAttributeValues; } }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
