using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class CSVManager : MonoBehaviour
{
    public string fileName;
    private List<string[]> rowData = new List<string[]>();

    private void Start()
    {
        string[] rowDataTemp = new string[25];


        rowDataTemp[0] = "mayorDeathReson";
        rowDataTemp[1] = "simulationTime";
        rowDataTemp[2] = "comfortMax";
        rowDataTemp[3] = "";
        rowDataTemp[4] = "collectorsCount";
        rowDataTemp[5] = "lumberjacksCount";
        rowDataTemp[6] = "artisansCount";
        rowDataTemp[7] = "babysCount";
        rowDataTemp[8] = "warehouseFoodCount";
        rowDataTemp[9] = "sceneFoodCount";
        rowDataTemp[10] = "warehouseWoodCount";
        rowDataTemp[11] = "sceneTreeCount";
        rowDataTemp[12] = "sceneWoodCount";
        rowDataTemp[13] = "collectorsDeathByMonster";
        rowDataTemp[14] = "collectorsDeathByHunger";
        rowDataTemp[15] = "collectorsDeathByThirst";
        rowDataTemp[16] = "lumberjacksDeathByMonster";
        rowDataTemp[17] = "lumberjacksDeathByHunger";
        rowDataTemp[18] = "lumberjacksDeathByThirst";
        rowDataTemp[19] = "artisansDeathByMonster";
        rowDataTemp[20] = "artisansDeathByHunger";
        rowDataTemp[21] = "artisansDeathByThirst";
        rowDataTemp[22] = "babysDeathByMonster";
        rowDataTemp[23] = "babysDeathByHunger";
        rowDataTemp[24] = "babysDeathByThirst";


        rowData.Add(rowDataTemp);

    }

    public void AddData(ResearchData researchData)
    {
        List<string[]> rowTmp = new List<string[]>();

        for (int i = 0; i < researchData.collectorsCount.Count; i++)
        {
            string[] rowDataTemp = new string[25];

            rowDataTemp[0] = researchData.mayorDeathReson[i].ToString();
            rowDataTemp[1] = researchData.simulationTime[i].ToString();
            rowDataTemp[2] = researchData.comfort[i].ToString();
            rowDataTemp[3] = "";

            rowDataTemp[4] = researchData.collectorsCount[i].ToString();
            rowDataTemp[5] = researchData.lumberjacksCount[i].ToString();
            rowDataTemp[6] = researchData.artisansCount[i].ToString();
            rowDataTemp[7] = researchData.babysCount[i].ToString();
            rowDataTemp[8] = researchData.warehouseFoodCount[i].ToString();
            rowDataTemp[9] = researchData.sceneFoodCount[i].ToString();
            rowDataTemp[10] = researchData.warehouseWoodCount[i].ToString();
            rowDataTemp[11] = researchData.sceneTreeCount[i].ToString();
            rowDataTemp[12] = researchData.sceneWoodCount[i].ToString();
            rowDataTemp[13] = researchData.collectorsDeathByMonster[i].ToString();
            rowDataTemp[14] = researchData.collectorsDeathByHunger[i].ToString();
            rowDataTemp[15] = researchData.collectorsDeathByThirst[i].ToString();
            rowDataTemp[16] = researchData.lumberjacksDeathByMonster[i].ToString();
            rowDataTemp[17] = researchData.lumberjacksDeathByHunger[i].ToString();
            rowDataTemp[18] = researchData.lumberjacksDeathByThirst[i].ToString();
            rowDataTemp[19] = researchData.artisansDeathByMonster[i].ToString();
            rowDataTemp[20] = researchData.artisansDeathByHunger[i].ToString();
            rowDataTemp[21] = researchData.artisansDeathByThirst[i].ToString();
            rowDataTemp[22] = researchData.babysDeathByMonster[i].ToString();
            rowDataTemp[23] = researchData.babysDeathByHunger[i].ToString();
            rowDataTemp[24] = researchData.babysDeathByThirst[i].ToString();
            rowTmp.Add(rowDataTemp);
        }
        rowTmp.Add(new string[25]);
        rowData.AddRange(rowTmp);
    }

    public void SaveData()
    {
        string[][] output = new string[rowData.Count][];

        for (int i = 0; i < rowData.Count; i++)
        {
            output[i] = rowData[i];// + rowMultipleData[i];
        }

        int length = output.GetLength(0);
        string delimiter = ";";

        StringBuilder sb = new StringBuilder();

        for (int index = 0; index < length; index++)
            sb.AppendLine(string.Join(delimiter, output[index]));


        StreamWriter outStream = System.IO.File.AppendText(GetPath("csv"));
        outStream.WriteLine(sb);
        outStream.Close();

        ParametersGiver parametersGiver = GetComponent<ParametersGiver>();
        string paramGiver = JsonUtility.ToJson(parametersGiver);

        outStream = System.IO.File.AppendText(GetPath("json"));
        outStream.WriteLine(paramGiver);
        outStream.Close();

    }

    private string GetPath(string type)
    {
        return Application.dataPath + "/Data/" + fileName + "." + type;
    }

}
