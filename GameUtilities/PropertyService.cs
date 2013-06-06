using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Threading.Tasks;

namespace GameUtilities
{
    public class PropertyService 
    {
        Dictionary<String, Dictionary<String, String>> data = new Dictionary<String, Dictionary<String, String>>();

        public void Init(String filename)
        {
            StreamReader input = new StreamReader(filename);

            data.Clear();

            try
            {
                for (;;)
                {
                    String GUID = input.ReadLine();
                    if (GUID.Length > 0)
                    {
                        Dictionary<String, String> dummyDict = new Dictionary<String, String>();

                        String dummy;
                        while ((dummy = input.ReadLine()).Length > 0)
                        {
                            int index = dummy.IndexOf(" : ");
                            String key = dummy.Remove(index);
                            String value = dummy.Remove(0, index + 3);

                            dummyDict.Add(key, value);
                        }

                        data.Add(GUID, dummyDict);
                    }
                }
            }
            catch (Exception e)
            {
            }
        }

        /*
        public void Init(String filename)
        {
            IModel m = IModel.Load(filename);
            data.Clear();

            foreach (IModelElement ele in m.Elemements)
            {
                for (int i = 0; i < ele.Classes.Count; i++)
                {
                    IModelClass c = ele.Classes[i];
                    IModelObject o = ele.Objects[i];

                    try
                    {
                        String GUID = o.ECInstance["GUID"].StringValue;
                        //String GUID = o.ECInstance["GlobalId"].StringValue;

                        Dictionary<String, String> dummyDict = new Dictionary<String, String>();

                        foreach (IModelProperty prpty in c.Properties)
                        {
                            if (o.ECInstance[prpty.Name] != null && !o.ECInstance[prpty.Name].IsNull)
                            {
                                if (o.ECInstance[prpty.Name].StringValue.Equals(String.Empty))
                                    continue;

                                if (o.ECInstance[prpty.Name].StringValue.Length <= 1)
                                    continue;

                                dummyDict.Add(prpty.Name, o.ECInstance[prpty.Name].StringValue);
                            }
                        }

                        data.Add(GUID, dummyDict);
                    }
                    catch (Exception e)
                    {
                    }
                }
            }
        }
        */
        public Dictionary<String, String> GetProperties(String key)
        {
            Dictionary<String, String> retVal = null;

            data.TryGetValue(key, out retVal);
            return retVal;
        }
    }
}
