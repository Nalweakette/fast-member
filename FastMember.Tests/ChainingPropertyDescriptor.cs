using System;
using System.ComponentModel;

namespace FastMember.Tests
{
    public abstract class ChainingPropertyDescriptor : PropertyDescriptor
    {
        #region "Members"

        private readonly PropertyDescriptor m_root;

        #endregion

        #region "Properties"

        public override AttributeCollection Attributes => this.Root.Attributes;

        public override string Category => this.Root.Category;

        public override Type ComponentType => this.Root.ComponentType;

        public override TypeConverter Converter => this.Root.Converter;

        public override string Description => this.Root.Description;

        public override bool DesignTimeOnly => this.Root.DesignTimeOnly;

        public override string DisplayName => this.Root.DisplayName;

        public override bool IsBrowsable => this.Root.IsBrowsable;

        public override bool IsLocalizable => this.Root.IsLocalizable;

        public override bool IsReadOnly => this.Root.IsReadOnly;

        public override string Name => this.Root.Name;

        public override Type PropertyType => this.Root.PropertyType;

        public override bool SupportsChangeEvents => this.Root.SupportsChangeEvents;

        protected PropertyDescriptor Root => this.m_root;

        #endregion

        #region "Constructors / Destructor"

        protected ChainingPropertyDescriptor(PropertyDescriptor root)
            : base(root)
        {
            this.m_root = root;
        }

        #endregion

        #region "Methods"

        public override void AddValueChanged(object component, EventHandler handler)
        {
            this.Root.AddValueChanged(component, handler);
        }

        public override bool CanResetValue(object component)
        {
            return this.Root.CanResetValue(component);
        }

        public override bool Equals(object obj)
        {
            return this.Root.Equals(obj);
        }

        public override PropertyDescriptorCollection GetChildProperties(object instance, Attribute[] filter)
        {
            return this.Root.GetChildProperties(instance, filter);
        }

        public override object GetEditor(Type editorBaseType)
        {
            return this.Root.GetEditor(editorBaseType);
        }

        public override int GetHashCode()
        {
            return this.Root.GetHashCode();
        }

        public override object GetValue(object component)
        {
            return this.Root.GetValue(component);
        }

        public override void RemoveValueChanged(object component, EventHandler handler)
        {
            this.Root.RemoveValueChanged(component, handler);
        }

        public override void ResetValue(object component)
        {
            this.Root.ResetValue(component);
        }

        public override void SetValue(object component, object value)
        {
            this.Root.SetValue(component, value);
        }

        public override bool ShouldSerializeValue(object component)
        {
            return this.Root.ShouldSerializeValue(component);
        }

        public override string ToString()
        {
            return this.Root.ToString();
        }

        #endregion
    }
}