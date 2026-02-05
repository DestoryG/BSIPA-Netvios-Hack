using System;
using System.Collections.Generic;
using System.Linq;
using BeatSaberMarkupLanguage.Parser;
using HMUI;
using UnityEngine;

namespace BeatSaberMarkupLanguage.Components.Settings
{
	// Token: 0x020000B0 RID: 176
	public class DropDownListSetting : GenericSetting, TableView.IDataSource
	{
		// Token: 0x170000ED RID: 237
		// (get) Token: 0x060003A2 RID: 930 RVA: 0x000117F1 File Offset: 0x0000F9F1
		// (set) Token: 0x060003A3 RID: 931 RVA: 0x0001180A File Offset: 0x0000FA0A
		public object Value
		{
			get
			{
				this.ValidateRange();
				return this.values[this.index];
			}
			set
			{
				this.index = this.values.IndexOf(value);
				if (this.index < 0)
				{
					this.index = 0;
				}
				this.UpdateState();
			}
		}

		// Token: 0x060003A4 RID: 932 RVA: 0x00011834 File Offset: 0x0000FA34
		public EnvironmentTableCell GetTableCell()
		{
			EnvironmentTableCell environmentTableCell = (EnvironmentTableCell)this.tableView.DequeueReusableCellForIdentifier(this.reuseIdentifier);
			if (!environmentTableCell)
			{
				if (this.tableCellInstance == null)
				{
					this.tableCellInstance = Resources.FindObjectsOfTypeAll<EnvironmentTableCell>().First<EnvironmentTableCell>();
				}
				environmentTableCell = Object.Instantiate<EnvironmentTableCell>(this.tableCellInstance);
			}
			environmentTableCell.reuseIdentifier = this.reuseIdentifier;
			return environmentTableCell;
		}

		// Token: 0x060003A5 RID: 933 RVA: 0x00011898 File Offset: 0x0000FA98
		public TableCell CellForIdx(TableView tableView, int idx)
		{
			EnvironmentTableCell tableCell = this.GetTableCell();
			tableCell.text = ((this.formatter == null) ? this.values[idx].ToString() : (this.formatter.Invoke(new object[] { this.values[idx] }) as string));
			return tableCell;
		}

		// Token: 0x060003A6 RID: 934 RVA: 0x000118F3 File Offset: 0x0000FAF3
		public float CellSize()
		{
			return 8f;
		}

		// Token: 0x060003A7 RID: 935 RVA: 0x000118FA File Offset: 0x0000FAFA
		public int NumberOfCells()
		{
			if (this.values == null)
			{
				return 0;
			}
			return this.values.Count<object>();
		}

		// Token: 0x060003A8 RID: 936 RVA: 0x00011911 File Offset: 0x0000FB11
		public override void Setup()
		{
			this.dropdown.didSelectCellWithIdxEvent += this.OnSelectIndex;
			this.ReceiveValue();
			this.dropdown.ReloadData();
			base.gameObject.SetActive(true);
		}

		// Token: 0x060003A9 RID: 937 RVA: 0x00011947 File Offset: 0x0000FB47
		private void OnSelectIndex(DropdownWithTableView tableView, int index)
		{
			this.index = index;
			this.UpdateState();
			BSMLAction onChange = this.onChange;
			if (onChange != null)
			{
				onChange.Invoke(new object[] { this.Value });
			}
			if (this.updateOnChange)
			{
				this.ApplyValue();
			}
		}

		// Token: 0x060003AA RID: 938 RVA: 0x00011985 File Offset: 0x0000FB85
		public override void ApplyValue()
		{
			if (this.associatedValue != null)
			{
				this.associatedValue.SetValue(this.Value);
			}
		}

		// Token: 0x060003AB RID: 939 RVA: 0x000119A0 File Offset: 0x0000FBA0
		public override void ReceiveValue()
		{
			if (this.associatedValue != null)
			{
				this.Value = this.associatedValue.GetValue();
				this.dropdown.SelectCellWithIdx(this.index);
			}
		}

		// Token: 0x060003AC RID: 940 RVA: 0x000119CC File Offset: 0x0000FBCC
		private void ValidateRange()
		{
			if (this.index >= this.values.Count)
			{
				this.index = this.values.Count - 1;
			}
			if (this.index < 0)
			{
				this.index = 0;
			}
		}

		// Token: 0x060003AD RID: 941 RVA: 0x00011A04 File Offset: 0x0000FC04
		private void UpdateState()
		{
			this.dropdown.SetValueText((this.formatter == null) ? this.Value.ToString() : (this.formatter.Invoke(new object[] { this.Value }) as string));
		}

		// Token: 0x04000117 RID: 279
		private string reuseIdentifier = "BSMLDropdownSetting";

		// Token: 0x04000118 RID: 280
		private EnvironmentTableCell tableCellInstance;

		// Token: 0x04000119 RID: 281
		private int index;

		// Token: 0x0400011A RID: 282
		public List<object> values;

		// Token: 0x0400011B RID: 283
		public TableView tableView;

		// Token: 0x0400011C RID: 284
		public LabelAndValueDropdownWithTableView dropdown;
	}
}
