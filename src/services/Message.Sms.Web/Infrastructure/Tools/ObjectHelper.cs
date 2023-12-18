using System.ComponentModel;
using System.Net;
using System.Text;

namespace Message.Sms;

public static class ObjectHelper
{
    /// <summary>
    /// 参数拼接Url
    /// </summary>
    /// <param name="source">要拼接的实体</param>
    /// <paramref name="IsStrUpper">是否开启首字母大写,默认小写</paramref>
    /// <returns>Url,</returns>
    public static string ToParameter(this object source, bool IsStrUpper = false)
    {
        var buff = new StringBuilder(string.Empty);
        if (source == null)
            throw new ArgumentNullException("source", "Unable to convert object to a dictionary. The source object is null.");
        foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(source))
        {
            object value = property.GetValue(source);
            if (value != null)
            {
                if (IsStrUpper)
                {
                    buff.Append(WebUtility.UrlEncode(property.Name) + "=" + WebUtility.UrlEncode(value + "") + "&");
                }
                else
                {
                    buff.Append(WebUtility.UrlEncode(GetlowerStr(property.Name)) + "=" + WebUtility.UrlEncode(value + "") + "&");
                }
            }
        }
        return buff.ToString().Trim('&');
    }
    /// <summary>
    /// 让首字母小写
    /// </summary>
    /// <param name="proname">类属性名称</param>
    /// <returns></returns>
    private static string GetlowerStr(object proname)
    {
        if (proname.ToString().Length >= 1)
        {
            return string.Format("{0}{1}", proname.ToString().Substring(0, 1).ToLower(), proname.ToString().Substring(1));
        }
        return string.Empty;
    }

    /// <summary>
    /// 获取一个类指定的属性值
    /// </summary>
    /// <param name="info">object对象</param>
    /// <param name="field">属性名称</param>
    /// <returns></returns>
    public static object GetPropertyValue(object info, string field)
    {
        if (info == null) return null;
        Type t = info.GetType();
        IEnumerable<System.Reflection.PropertyInfo> property = from pi in t.GetProperties() where pi.Name.ToLower() == field.ToLower() select pi;
        return property.First().GetValue(info, null);
    }

}