using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;

namespace InventoryManagement.DataModel
{
    public class Item : INotifyPropertyChanged
    {
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }

        private string _ItemNo = string.Empty;
        public string ItemNo
        {
            get
            {
                return this._ItemNo;
            }
            set
            {
                if (this._ItemNo != value)
                {
                    this._ItemNo = value;
                    this.OnPropertyChanged("ItemNo");
                }
            }
        }
        private string _Description = string.Empty;
        public string Description
        {
            get
            {
                return this._Description;
            }
            set
            {
                if (this._Description != value)
                {
                    this._Description = value;
                    this.OnPropertyChanged("Description");
                }
            }
        }
        private string _ItemClass = string.Empty;
        public string ItemClass
        {
            get
            {
                return this._ItemClass;
            }
            set
            {
                if (this._ItemClass != value)
                {
                    this._ItemClass = value;
                    this.OnPropertyChanged("ItemClass");
                }
            }
        }
        private decimal _UnitCost;
        public decimal UnitCost
        {
            get
            {
                return this._UnitCost;
            }
            set
            {
                if (this._UnitCost != value)
                {
                    this._UnitCost = value;
                    this.OnPropertyChanged("UnitCost");
                }
            }
        }
        private decimal _QuantityIssued;
        public decimal QuantityIssued
        {
            get
            {
                return this._QuantityIssued;
            }
            set
            {
                if (this._QuantityIssued != value)
                {
                    this._QuantityIssued = value;
                    this.OnPropertyChanged("QuantityIssued");
                }
            }
        }
        private decimal _QuantityInHand;
        public decimal QuantityInHand
        {
            get
            {
                return this._QuantityInHand;
            }
            set
            {
                if (this._QuantityInHand != value)
                {
                    this._QuantityInHand = value;
                    this.OnPropertyChanged("QuantityInHand");
                }
            }
        }
        private decimal _ItemType;
        public decimal ItemType
        {
            get
            {
                return this._ItemType;
            }
            set
            {
                if (this._ItemType != value)
                {
                    this._ItemType = value;
                    this.OnPropertyChanged("ItemType");
                }
            }
        }
    }
}

