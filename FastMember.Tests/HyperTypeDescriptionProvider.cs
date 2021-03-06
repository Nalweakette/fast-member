using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Security.Permissions;

/* Change history:
 * 20 Apr 2007  Marc Gravell    Rollback dictionary on error;
 *                              Assert ReflectionPermission for main creation
 *                                  (thanks/credit to Josh Smith for feedback/hints)
 */

namespace FastMember.Tests
{
    public sealed class HyperTypeDescriptionProvider : TypeDescriptionProvider
    {
        #region "Members"

        private static readonly Dictionary<Type, ICustomTypeDescriptor> descriptors = new Dictionary<Type, ICustomTypeDescriptor>();

        #endregion

        #region "Constructors / Destructor"

        public HyperTypeDescriptionProvider()
            : this(typeof(object))
        {
        }

        public HyperTypeDescriptionProvider(Type type)
            : this(TypeDescriptor.GetProvider(type))
        {
        }

        public HyperTypeDescriptionProvider(TypeDescriptionProvider parent)
            : base(parent)
        {
        }

        #endregion

        #region "Static Methods"

        public static void Add(Type type)
        {
            TypeDescriptionProvider parent = TypeDescriptor.GetProvider(type);
            TypeDescriptor.AddProvider(new HyperTypeDescriptionProvider(parent), type);
        }

        public static void Clear(Type type)
        {
            lock (descriptors)
            {
                descriptors.Remove(type);
            }
        }

        public static void Clear()
        {
            lock (descriptors)
            {
                descriptors.Clear();
            }
        }

        #endregion

        #region "Methods"

        public sealed override ICustomTypeDescriptor GetTypeDescriptor(Type objectType, object instance)
        {
            ICustomTypeDescriptor descriptor;
            lock (descriptors)
            {
                if (!descriptors.TryGetValue(objectType, out descriptor))
                {
                    try
                    {
                        descriptor = this.BuildDescriptor(objectType);
                    }
                    catch
                    {
                        return base.GetTypeDescriptor(objectType, instance);
                    }
                }

                return descriptor;
            }
        }
#pragma warning disable CS0618, CS0612
        [ReflectionPermission(SecurityAction.Assert, Flags = ReflectionPermissionFlag.AllFlags)]
#pragma warning restore CS0618, CS0612
        private ICustomTypeDescriptor BuildDescriptor(Type objectType)
        {
            // NOTE: "descriptors" already locked here

            // get the parent descriptor and add to the dictionary so that
            // building the new descriptor will use the base rather than recursing
            ICustomTypeDescriptor descriptor = base.GetTypeDescriptor(objectType, null);
            descriptors.Add(objectType, descriptor);
            try
            {
                // build a new descriptor from this, and replace the lookup
                descriptor = new HyperTypeDescriptor(descriptor);
                descriptors[objectType] = descriptor;
                return descriptor;
            }
            catch
            {
                // rollback and throw
                // (perhaps because the specific caller lacked permissions;
                // another caller may be successful)
                descriptors.Remove(objectType);
                throw;
            }
        }

        #endregion
    }
}