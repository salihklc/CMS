using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace CMS.Common.Extensions
{
    public static class ObjectExtension
    {
        public static T xClone<T>(this T poObject)
        {
            if (!typeof(T).IsSerializable)
            {
                throw new Exception("The type must be Serializable");
            }

            if (Object.ReferenceEquals(poObject, null))
            {
                return default(T);
            }

            using (MemoryStream memoryStream = new MemoryStream())
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                binaryFormatter.Serialize(memoryStream, poObject);

                memoryStream.Seek(0, SeekOrigin.Begin);
                return (T)binaryFormatter.Deserialize(memoryStream);
            }
        }

        public static string xSerializeAsXml(this object poObject)
        {
            try
            {
                if (poObject == null)
                    return "";

                XmlSerializer xmlSerializer = new XmlSerializer(poObject.GetType());

                using (StringWriter stringWriter = new StringWriter())
                {
                    XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
                    namespaces.Add("", "");

                    xmlSerializer.Serialize(stringWriter, poObject, namespaces);

                    XElement root = XElement.Parse(stringWriter.ToString());

                    root.Descendants("Password").Remove();
                    root.Descendants("UserPassword").Remove();
                    root.Descendants(XName.Get("Password", "http://tempuri.org/")).Remove();
                    root.Descendants(XName.Get("UserPassword", "http://tempuri.org/")).Remove();

                    return root.ToString();
                }
            }
            catch (Exception e)
            {
                return "";
            }
        }

        public static T xCopyTo<T>(this object poSource)
        {
            return (T)xCopyTo(poSource, typeof(T));
        }

        public static object xCopyTo(this object poSource, Type poTargetType)
        {
            object poTarget = Activator.CreateInstance(poTargetType);
            xCopyTo(poSource, poTarget);

            return poTarget;
        }

        public static void xCopyTo(this object poSource, object poTarget)
        {
            if (Object.ReferenceEquals(poSource, null))
                return;

            if (Object.ReferenceEquals(poTarget, null))
                return;

            Type objSrcType = poSource.GetType();
            Type objDstType = poTarget.GetType();

            foreach (PropertyInfo objSrcProp in objSrcType.GetProperties())
            {
                PropertyInfo objDstProp = objDstType.GetProperty(objSrcProp.Name);

                if (objDstProp == null)
                    continue;

                #region dst => list
                if (objDstProp.PropertyType.xIsList())
                {
                    if (objSrcProp.GetValue(poSource) == null)
                        continue;

                    #region src => list
                    if (objSrcProp.PropertyType.xIsList())
                    {
                        int iSrcLen = (objSrcProp.GetValue(poSource) as IList).Count;

                        for (int i = 0; i < iSrcLen; i++)
                        {
                            object objItem = objDstProp.PropertyType.xCreateListItem();
                            (objSrcProp.GetValue(poSource) as IList)[i].xCopyTo(objItem);

                            if (objDstProp.GetValue(poTarget) == null)
                            {
                                objDstProp.SetValue(poTarget, objItem.GetType().xCreateList());
                            }

                            (objDstProp.GetValue(poTarget) as IList).Add(objItem);
                        }
                    }
                    #endregion src => list

                    #region src => array
                    else if (objSrcProp.PropertyType.xIsArray())
                    {
                        int iSrcLen = (objSrcProp.GetValue(poSource) as Array).Length;

                        for (int i = 0; i < iSrcLen; i++)
                        {
                            object objItem = objDstProp.PropertyType.xCreateListItem();
                            (objSrcProp.GetValue(poSource) as Array).GetValue(i).xCopyTo(objItem);

                            if (objDstProp.GetValue(poTarget) == null)
                            {
                                objDstProp.SetValue(poTarget, objItem.GetType().xCreateList());
                            }

                            (objDstProp.GetValue(poTarget) as IList).Add(objItem);
                        }
                    }
                    #endregion src => array
                }
                #endregion dst => list

                #region dst => array
                else if (objDstProp.PropertyType.xIsArray())
                {
                    if (objSrcProp.GetValue(poSource) == null)
                        continue;

                    #region src => array
                    if (objSrcProp.PropertyType.xIsArray())
                    {
                        int iSrcLen = (objSrcProp.GetValue(poSource) as Array).Length;

                        Array arrDst = objDstProp.PropertyType.xCreateArray(iSrcLen);
                        objDstProp.SetValue(poTarget, arrDst);

                        for (int i = 0; i < iSrcLen; i++)
                        {
                            object objItem = objDstProp.PropertyType.xCreateArrayItem();
                            (objSrcProp.GetValue(poSource) as Array).GetValue(i).xCopyTo(objItem);

                            (objDstProp.GetValue(poTarget) as Array).SetValue(objItem, i);
                        }
                    }
                    #endregion src => array

                    #region src => list
                    else if (objSrcProp.PropertyType.xIsList())
                    {
                        int iSrcLen = (objSrcProp.GetValue(poSource) as IList).Count;

                        for (int i = 0; i < iSrcLen; i++)
                        {
                            object objItem = objDstProp.PropertyType.xCreateListItem();
                            (objSrcProp.GetValue(poSource) as IList)[i].xCopyTo(objItem);

                            if (objDstProp.GetValue(poTarget) == null)
                            {
                                objDstProp.SetValue(poTarget, objItem.GetType().xCreateArray(iSrcLen));
                            }

                            (objDstProp.GetValue(poTarget) as Array).SetValue(objItem, i);
                        }
                    }
                    #endregion src => list
                }
                #endregion dst => array

                #region dst => object
                else if (objDstProp.PropertyType.xIsSingleObject())
                {
                    if (!objSrcProp.PropertyType.xIsSingleObject())
                        continue;

                    if (objSrcProp.GetValue(poSource) == null)
                        continue;

                    object objItem = objSrcProp.GetValue(poSource).xCopyTo(objDstProp.PropertyType);
                    objDstProp.SetValue(poTarget, objItem);
                }
                #endregion object

                #region dst => other
                else
                {
                    if (objSrcProp.PropertyType == objDstProp.PropertyType)
                    {
                        objDstProp.SetValue(poTarget, objSrcProp.GetValue(poSource));
                    }
                    else if (objDstProp.PropertyType.IsEnum || objSrcProp.PropertyType.IsEnum)
                    {
                        objDstProp.SetValue(poTarget, objSrcProp.GetValue(poSource));
                    }
                    else
                    {
                        objDstProp.SetValue(poTarget, Convert.ChangeType(objSrcProp.GetValue(poSource), objDstProp.PropertyType));
                    }
                }
                #endregion primitive
            }
        }
    }
}
