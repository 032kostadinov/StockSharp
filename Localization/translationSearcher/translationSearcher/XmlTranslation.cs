using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Web;

///	<summary/>
public class XmlTranslation
{
	/// <summary>
	/// ��� ������� �� �������� � <paramref name="xmlFile"/> ����� ������� � ����� <paramref name="textCsvFile"/>. 
	/// ���� ������ - ������� ������������� �������� � ���� �������� (������� 0) � ���� <paramref name="resultIdCsvFile"/>,
	/// ����� - ��������� ���� <paramref name="textCsvFile"/> ������� �� ��������������� ��������������� �������� � ������� ��������
	/// ���������������� ��� ����� ������. ��������������, ��� ���� �������� � <paramref name="xmlFile"/> ������������� ������� � 
	/// �������� 2 � ����� <paramref name="textCsvFile"/>
	/// </summary>
	/// <param name="xmlFile">���� � ������������� �����, ������� ���������� ��������������</param>
	/// <param name="textCsvFile">���� � ����� https://github.com/StockSharp/StockSharp/blob/master/Localization/text.csv �� �����</param>
	/// <param name="resultIdCsvFile">����, ��� ������ ���� ������ (��� ����������� � ����) ����</param>
	public void SearchTranslationsForXml(string xmlFile, string textCsvFile, string resultIdCsvFile)
	{
		int newKeyCount = 0;

		var newKeyIndex = parseTextCsv(textCsvFile, m_TranslationKeys, 2);	//	������������, ��� �������� ���� �� �������, �.�. ������� 2 � text.csv
		endOfLineCorrection(textCsvFile);	//	�������� csv �� ������������ �� \n
		var outStreamTextCsv = new StreamWriter(textCsvFile, true);

		var outStream = new StreamWriter(resultIdCsvFile);

		var root = XDocument.Load(xmlFile);
		var members = root.Elements("doc").Elements("members").Elements("member");

		var newTranslations = new HashSet<string>();
		foreach (var member in members)
		{
			var name = member.Attribute("name");
			foreach (var tag in member.Elements())
			{
				XElement []tagChildren;
				var content = parseContent(tag, out tagChildren);
				if (!m_TranslationKeys.ContainsKey(content))
				{
					if (newTranslations.Contains(content))
						continue;
					newTranslations.Add(content);

					if (content.Contains(";"))
						content = "\"" + content + "\"";

					var newKey = prefix + newKeyIndex++.ToString();
					outStreamTextCsv.WriteLine(newKey + ";" + content + ";" + content);

					newKeyCount++;
				}
				else
					outStream.WriteLine(getPath(tag) + ";" + m_TranslationKeys[content].Item1);
			}
		}

		outStream.Close();
		outStreamTextCsv.Close();

		if (newKeyCount != 0)
			Console.WriteLine("�� ������� " + newKeyCount.ToString() + " ���������");
		else
			Console.WriteLine("��� �������� �������");
	}

	/// <summary>
	/// ��������� �������� � <paramref name="xmlFile"/> �� ����, ��������������� ������� <paramref name="language"/> � ����� 
	/// <paramref name="textCsvFile"/>, ���������� �������������� �������� ������ �������� �� ������ ����� 
	/// <paramref name="idCsvFile"/>
	/// </summary>
	/// <param name="xmlFile">���� � ������������� �����, ������� ���������� ������������ ��� ��������� ��� �������� <paramref name="resultXmlFile"/></param>
	/// <param name="idCsvFile">���� � ������������� �����, ��������� ����� <see cref="SearchTranslationsForXml"/></param> 
	/// <param name="textCsvFile"> ���� � ����� https://github.com/StockSharp/StockSharp/blob/master/Localization/text.csv �� �����</param>
	/// <param name="language">����� ������� � 1 ���������� ��� ����� � ����� <paramref name="textCsvFile"/></param>
	/// <param name="resultXmlFile">����, ��� ������ ���� ������ (��� ����������� � ����) ����</param>
	public void TranslateXml(string xmlFile, string idCsvFile, string textCsvFile, int language, string resultXmlFile)
	{
		parseTextCsv(textCsvFile, m_Translations, 0);	//	���� ������� - ������ ������� � �����, �.�. ���������� ���� ��������
		m_TranslKeysForPath = parseIdCsv(idCsvFile);

		var root = XDocument.Load(xmlFile);
		var members = root.Elements("doc").Elements("members").Elements("member");

		foreach (var member in members)
		{
			var name = member.Attribute("name");
			foreach (var tag in member.Elements().ToArray())
				translate(tag, language, xmlFile, idCsvFile, textCsvFile);
		}

		root.Save(resultXmlFile);
	}

	private const string prefix = "DocStr";
	private Dictionary<string, Tuple<string, string>> m_Translations = new Dictionary<string, Tuple<string, string>>();	//	���� ������� - ���� �������� �� text.csv, Tuple - ��������
	private Dictionary<string, Tuple<string, string>> m_TranslationKeys = new Dictionary<string, Tuple<string, string>>();	//	���� ������� - ������� ������� (������� 2 � ����� text.csv), Tuple.Item1 - ���� �������� �� text.csv
	private Dictionary<string, string> m_TranslKeysForPath;	//	���� ������� - ���� �� ����� idCsvFile, �������� - ��������������� ���� �������� �� text.csv

	private void translate(XElement tag, int language, string xmlFile, string idCsvFile, string textCsvFile)
	{
		var tagPath = getPath(tag);
		if (!m_TranslKeysForPath.ContainsKey(tagPath))
			throw new Exception("� ����� " + idCsvFile + " �� ������ ���� " + tagPath +
				" ������������� �������� <member> � ����� " + xmlFile + ", ���������� ���������������� �������");

		var translKey = m_TranslKeysForPath[tagPath];

		var original = tag.ToString();

		XElement[] tagChildren;
		var content = parseContent(tag, out tagChildren);

		if (!m_Translations.ContainsKey(translKey))
			throw new Exception("� ����� " + textCsvFile + " �� ������ ������� �� ����� " + translKey);

		if (language < 1 || language > 2)
			throw new Exception("������ ����� �������� �� 1 ��� 2 �� ��������������");
		var translation = language == 1 ? m_Translations[translKey].Item1 : m_Translations[translKey].Item2;

		var encoded = HttpUtility.HtmlEncode(translation);	//	��������, ���� ���������� "����� (X&amp;0)", �������������� � XElement.Value � "����� (X&0)", � ���� ����� ����������� �������

		var newContent = "";
		if (tagChildren.Count() != 0)
			try
			{
				newContent = string.Format(encoded, tagChildren);
			}
			catch (FormatException)
			{
				throw new Exception("�� ������� ����������� xml ���� �������� ������ ������� ��������:\n�������� - " + original +
					"\n������� - " + encoded);
			}
		else	//	����������� �������� ������ ��� ����������, �� � ��������� ��������
			newContent = encoded;

		var newText = "<" + tag.Name + ">" + newContent + "</" + tag.Name + ">";
		var newEl = XElement.Parse(newText);
		newEl.Add(tag.Attributes());

		tag.ReplaceWith(newEl);
	}

	private string cleanString(string s)
	{
		var ss = s.Split('\n');
		for (int i = 0; i < ss.Count(); i++ )
			ss[i] = ss[i].Trim();
		s = string.Join(" ", ss).Trim();
		return s.TrimEnd('.').Trim();
	}

	private string parseContent(XElement tag, out XElement []tagChildren)
	{
		tagChildren = tag.Elements().ToArray();

		int index = 0;
		while (tag.Elements().Count() != 0)
			tag.Elements().First().ReplaceWith("{" + index++ + "}");

		return cleanString(tag.Value);
	}

	private int parseTextCsv(string fName, Dictionary<string, Tuple<string, string>> res, uint dictKeyColIndex)
	{
		var file = new StreamReader(fName);
		string line;

		string[] ss = null;

		while ((line = file.ReadLine()) != null)
		{
			if (line.Contains(";\""))	//	��������: Str3634Params;"New candle {0}: {6} {1};{2};{3};{4}; volume {5}";"����� ����� {0}: {6} {1};{2};{3};{4}; ����� {5}"
			{
				ss = line.Split(new string[]{";\""}, StringSplitOptions.None);
				if (ss.Length != 3)
					throw new Exception("text.csv �������� ������������ ������: \"" + line + "\"");
				ss[1] = ss[1].TrimEnd('\"');
				ss[2] = ss[2].TrimEnd('\"');
			}
			else
			{
				ss = line.Split(';');
				if (ss.Length != 3)
					throw new Exception("text.csv �������� ������������ ������: \"" + line + "\"");
			}

			ss[1] = cleanString(ss[1]);
			ss[2] = cleanString(ss[2]);

			if (dictKeyColIndex > 2)
				throw new Exception("������������ ����� parseTextCsv (��� � ����)");
			var indices = new List<uint> { 0, 1, 2 };
			indices.Remove(dictKeyColIndex);	//	ss ��� ���������� ���� �������� - � �������� Dictionary

			res[ss[dictKeyColIndex]] = new Tuple<string, string>(ss[indices[0]], ss[indices[1]]);
		}
		file.Close();

		//	���������� ������, � �������� ����� �������� ��������� ����� ������ <prefix>.. � text.csv
		if (ss != null && ss[0].StartsWith(prefix))
			return int.Parse(ss[0].Substring(prefix.Length)) + 1;
		else
			return 0;
	}

	private Dictionary<string, string> parseIdCsv(string fName)
	{
		var file = new StreamReader(fName);
		string line;

		string[] ss = null;
		var res = new Dictionary<string, string>();

		while ((line = file.ReadLine()) != null)
		{
			ss = line.Split(';');
			if (ss.Length != 2)
				throw new Exception(fName + " �������� ������������ ������: \"" + line + "\"");

			res[ss[0]] = ss[1];
		}
		file.Close();
		return res;
	}

	private void endOfLineCorrection(string fName)
	{
		if (!File.Exists(fName))
			return;
		var fi = new FileInfo(fName);
		if (fi.Length == 0)
			return;
			
		var f = File.Open(fName, FileMode.Open, FileAccess.ReadWrite);
		f.Seek(-1, SeekOrigin.End);
		char c = (char)f.ReadByte();
		if (c != '\n')
			f.WriteByte((byte)'\n');

		f.Close();
	}

	private string getPath(XElement tag)
	{
		var basic = tag.Parent.Attribute("name").Value + "/" + tag.Name;
		var nameAttr = tag.Attribute("name");
		if (nameAttr != null)
			return basic + "/" + nameAttr.Value;
		else
			return basic;
	}
};


