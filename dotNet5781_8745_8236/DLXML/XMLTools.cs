﻿using System;
using System.Collections.Generic;
using System.Xml.Linq;
using System.IO;
using System.Xml.Serialization;
namespace DL
{
    /// <summary>
    /// Tools class for xml
    /// </summary>
    public class XMLTools
    {
        /// <summary>
        /// Start for every file path, inside 'xml' folder
        /// </summary>
        static string dir = @"xml\";

        /// <summary>
        /// Static constructor
        /// </summary>
        static XMLTools()
        {
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
        }

        #region SaveLoadWithXElement
        /// <summary>
        /// Saving XElement in xml file
        /// </summary>
        /// <param name="rootElem">XElement to save</param>
        /// <param name="filePath">The file's path</param>
        public static void SaveListToXMLElement(XElement rootElem, string filePath)
        {
            try
            {
                rootElem.Save(dir + filePath);
            }
            catch (Exception ex)
            {
                throw new DO.XMLFileLoadCreateException(filePath, $"fail to create xml file: {filePath}", ex);
            }
        }
        /// <summary>
        /// Load XElement from file
        /// </summary>
        /// <param name="filePath">File's path</param>
        /// <returns>The XElement (root) in the file</returns>
        public static XElement LoadListFromXMLElement(string filePath)
        {
            try
            {
                if (File.Exists(dir + filePath))
                {
                    return XElement.Load(dir + filePath);
                }
                else
                {
                    XElement rootElem = new XElement(dir + filePath);
                    rootElem.Save(dir + filePath);
                    return rootElem;
                }
            }
            catch (Exception ex)
            {
                throw new DO.XMLFileLoadCreateException(filePath, $"fail to load xml file: {filePath}", ex);
            }
        }
        #endregion

        #region SaveLoadWithXMLSerializer
        /// <summary>
        /// Save generic list in xml file using Serializer
        /// </summary>
        /// <typeparam name="T">Class possible for Serializer</typeparam>
        /// <param name="list">Generic list</param>
        /// <param name="filePath">File's path</param>
        public static void SaveListToXMLSerializer<T>(List<T> list, string filePath)
        {
            try
            {
                FileStream file = new FileStream(dir + filePath, FileMode.Create);
                XmlSerializer x = new XmlSerializer(list.GetType());
                x.Serialize(file, list);
                file.Close();
            }
            catch (Exception ex)
            {
                throw new DO.XMLFileLoadCreateException(filePath, $"fail to create xml file: {filePath}", ex);
            }
        }
        /// <summary>
        /// Load generic list from xml file using Serializer
        /// </summary>
        /// <typeparam name="T">Class possible for Serializer</typeparam>
        /// <param name="filePath">File's path</param>
        /// <returns>Generic list from the file</returns>
        public static List<T> LoadListFromXMLSerializer<T>(string filePath)
        {
            try
            {
                if (File.Exists(dir + filePath))
                {
                    List<T> list;
                    XmlSerializer x = new XmlSerializer(typeof(List<T>));
                    FileStream file = new FileStream(dir + filePath, FileMode.Open);
                    list = (List<T>)x.Deserialize(file);
                    file.Close();
                    return list;
                }
                else
                    return new List<T>();
            }
            catch (Exception ex)
            {
                throw new DO.XMLFileLoadCreateException(filePath, $"fail to load xml file: {filePath}", ex);
            }
        }
        #endregion
    }
}
