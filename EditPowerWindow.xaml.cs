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
using System.ComponentModel;
using System.IO;

namespace CharPad
{
    /// <summary>
    /// Interaction logic for EditPowerWindow.xaml
    /// </summary>
    public partial class EditPowerWindow : Window, INotifyPropertyChanged
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

        private BitmapSource powerImage;

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

            attackTypes = BindableEnum<PowerAttackType>.BuildValues(null, new List<PowerAttackType> { PowerAttackType.None });

            attackWeaponValues = BindableEnum<WeaponSlot>.BuildValues(new Dictionary<WeaponSlot, string>
            {
                { WeaponSlot.MainWeapon, "Main Weapon" },
                { WeaponSlot.OffhandWeapon, "Off-hand Weapon" },
                { WeaponSlot.RangedWeapon, "Ranged Weapon" }
            });

            attackAttributeValues = BindableEnum<AttributeType>.BuildValues(null, new List<AttributeType> { AttributeType.Wildcard });

            defenseTypeValues = BindableEnum<DefenseType>.BuildValues();

            bonusDamageAttributeValues = BindableEnum<AttributeType>.BuildValues(null, new List<AttributeType> { AttributeType.Wildcard });
            bonusDamageAttributeValues.Insert(0, new BindableEnum<AttributeType>("N/A", (AttributeType)(-1)));

            InitializeComponent();

            AttackAdjustments = new BasicAdjustmentList();
            DamageAdjustments = new BasicAdjustmentList();

            if (power == null)
            {
                txtLevel.Text = "1";
                cboPowerType.SelectedItem = powerTypes.Find(x => x.Value == PowerType.AtWill);
                cboActionType.SelectedItem = actionTypes.Find(x => x.Value == PowerActionType.Standard);
                chkIsActionPower.IsChecked = true;
                cboAttackType.SelectedItem = attackTypes.Find(x => x.Value == PowerAttackType.Weapon);
                cboWeapon.SelectedItem = attackWeaponValues.Find(x => x.Value == WeaponSlot.MainWeapon);
                cboAttackAttr.SelectedItem = attackAttributeValues.Find(x => x.Value == AttributeType.Strength);
                cboDefenseType.SelectedItem = defenseTypeValues.Find(x => x.Value == DefenseType.AC);
                cboBonusDamageAttr.SelectedItem = bonusDamageAttributeValues.Find(x => (int)x.Value == -1);
            }
            else
            {
                txtName.Text = power.Name;
                txtLevel.Text = power.Level.ToString();
                txtDescription.Text = power.Description;
                cboPowerType.SelectedItem = powerTypes.Find(x => x.Value == power.PowerType);
                cboActionType.SelectedItem = actionTypes.Find(x => x.Value == power.ActionType);
                txtNotes.Text = power.Notes;
                PowerImage = Common.BuildBitmapImage(power.Picture);

                if (power.AttackType == PowerAttackType.None)
                    chkIsActionPower.IsChecked = false;
                else
                {
                    chkIsActionPower.IsChecked = true;

                    cboAttackType.SelectedItem = attackTypes.Find(x => x.Value == power.AttackType);
                    cboWeapon.SelectedItem = attackWeaponValues.Find(x => x.Value == power.AttackWeapon);
                    cboAttackAttr.SelectedItem = attackAttributeValues.Find(x => x.Value == power.AttackAttribute);
                    cboDefenseType.SelectedItem = defenseTypeValues.Find(x => x.Value == power.DefenseType);
                    txtDamage.Text = (power.AttackWeapon == WeaponSlot.Implement ? (power.Damage == null ? "" : power.Damage.DisplayString) : power.WeaponDamamgeMultiplier.ToString());
                    txtDamageType.Text = power.DamageType;

                    if (power.BonusDamageAttribute == null)
                        cboBonusDamageAttr.SelectedItem = bonusDamageAttributeValues.Find(x => (int)x.Value == -1);
                    else
                        cboBonusDamageAttr.SelectedItem = bonusDamageAttributeValues.Find(x => x.Value == power.BonusDamageAttribute.Value);

                    foreach (BasicAdjustment adjustment in power.AttackModifiers)
                    {
                        attackAdjustments.Add(new BasicAdjustment(adjustment));
                    }

                    foreach (BasicAdjustment adjustment in power.DamageModifiers)
                    {
                        damageAdjustments.Add(new BasicAdjustment(adjustment));
                    }
                }
            }
        }

        public List<BindableEnum<PowerType>> PowerTypes { get { return powerTypes; } }
        public List<BindableEnum<PowerActionType>> ActionTypes { get { return actionTypes; } }
        public List<BindableEnum<PowerAttackType>> AttackTypes { get { return attackTypes; } }
        public List<BindableEnum<WeaponSlot>> AttackWeaponValues { get { return attackWeaponValues; } }
        public List<BindableEnum<AttributeType>> AttackAttributeValues { get { return attackAttributeValues; } }
        public List<BindableEnum<DefenseType>> DefenseTypeValues { get { return defenseTypeValues; } }
        public List<BindableEnum<AttributeType>> BonusDamageAttributeValues { get { return bonusDamageAttributeValues; } }

        public BasicAdjustmentList AttackAdjustments { get { return attackAdjustments; } set { attackAdjustments = value; Notify("AttackAdjustments"); } }
        public BasicAdjustmentList DamageAdjustments { get { return damageAdjustments; } set { damageAdjustments = value; Notify("DamageAdjustments"); } }

        public BitmapSource PowerImage { get { return powerImage; } set { powerImage = value; Notify("WeaponImage"); } }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Please enter a power name.", "Invalid name", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtName.Focus();
                return;
            }

            int level;

            if (String.IsNullOrWhiteSpace(txtLevel.Text) || !Int32.TryParse(txtLevel.Text, out level))
            {
                MessageBox.Show("Please enter a valid level.", "Invalid level", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtLevel.Focus();
                return;
            }

            if (chkIsActionPower.IsChecked.Value)
            {
                if (!IsDamageOfWeaponSpecifierType)
                {
                    if (!Dice.IsValidString(txtDamage.Text))
                    {
                        MessageBox.Show("Please enter a valid damage.", "Invalid damage", MessageBoxButton.OK, MessageBoxImage.Warning);
                        txtDamage.Focus();
                        return;
                    }
                }
            }

            DialogResult = true;
        }

        private bool IsDamageOfWeaponSpecifierType
        {
            get { return ((Visibility)(new CharPad.ValueConverters.PowerDamageLabelConverter()).Convert(txtDamage.Text, typeof(string), null, null) == Visibility.Visible); }
        }

        public void UpdatePower(Power power)
        {
            power.Name = txtName.Text;
            power.Level = Convert.ToInt32(txtLevel.Text);
            power.Description = txtDescription.Text;
            power.PowerType = ((BindableEnum<PowerType>)cboPowerType.SelectedItem).Value;
            power.ActionType = ((BindableEnum<PowerActionType>)cboActionType.SelectedItem).Value;
            power.Notes = txtNotes.Text;
            power.Picture = Common.ConvertToBitmap(powerImage);
            power.AttackType = (!chkIsActionPower.IsChecked.Value ? PowerAttackType.None : ((BindableEnum<PowerAttackType>)cboAttackType.SelectedItem).Value);

            if (power.AttackType != PowerAttackType.None)
            {
                power.AttackWeapon = ((BindableEnum<WeaponSlot>)cboWeapon.SelectedItem).Value;
                power.AttackAttribute = ((BindableEnum<AttributeType>)cboAttackAttr.SelectedItem).Value;
                power.DefenseType = ((BindableEnum<DefenseType>)cboDefenseType.SelectedItem).Value;

                if (IsDamageOfWeaponSpecifierType)
                    power.WeaponDamamgeMultiplier = Convert.ToInt32(txtDamage.Text);
                else
                    power.Damage = Dice.GetFromString(txtDamage.Text);

                power.DamageType = txtDamageType.Text;

                AttributeType bonusDamageAttr = ((BindableEnum<AttributeType>)cboBonusDamageAttr.SelectedItem).Value;
                power.BonusDamageAttribute = ((int)bonusDamageAttr == -1 ? null : (AttributeType?)bonusDamageAttr);

                power.AttackModifiers.Clear();

                foreach (BasicAdjustment adjustment in attackAdjustments)
                {
                    power.AttackModifiers.Add(new BasicAdjustment(adjustment));
                }

                power.DamageModifiers.Clear();

                foreach (BasicAdjustment adjustment in damageAdjustments)
                {
                    power.DamageModifiers.Add(new BasicAdjustment(adjustment));
                }
            }
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        private void Notify(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
