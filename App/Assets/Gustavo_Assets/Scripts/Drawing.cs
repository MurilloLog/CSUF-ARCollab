using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Drawing
{
    // Datos del ancla
    public string command;
    public string _id;
    public string roomId;
    public string anchorID;
    public Vector3 anchorPosition;
    public Quaternion anchorRotation;

    // Datos de la linea
    public List<Vector3> linePoints;
    public int lineColor;
    public long T1, T2; // Latencia de envio
    public long T3, T4; // Latencia de recepcion
    public long T5; // Latencia de procesamiento y renderizado
    public int np;

    public Drawing(string playerId, string roomID, string anchorId, Vector3 position, Quaternion rotation, List<Vector3> points, int color, long timestampT1, int numberOfPoints)
    {
        command = "DRAWING";
        _id = playerId;
        roomId = roomID;
        anchorID = anchorId;
        anchorPosition = position;
        anchorRotation = rotation;
        linePoints = points;
        lineColor = color;
        T1 = timestampT1;
        np = numberOfPoints;
    }

    public void SetTimestampT2(long timestampT2) => T2 = timestampT2;
    public void SetTimestampT3(long timestampT3) => T3 = timestampT3;
    public void SetTimestampT4(long timestampT4) => T4 = timestampT4;
    public void SetTimestampT5(long timestampT5) => T5 = timestampT5;

    public void SaveLatencyData()
    {
        if (T1 == 0 || T2 == 0 || T3 == 0 || T4 == 0 || T5 == 0)
        {
            Debug.LogWarning("No se pueden calcular las latencias porque algunos timestamps no han sido inicializados.");
            return;
        }

        // Cálculo de latencias
        long latencySend = T2 - T1;
        long latencyTransmit = T3 - T2;
        long latencyRelay = T4 - T3;
        long latencyRebuild = T5 - T4;
        long latencyTotal = T5 - T1;

        // Ruta de almacenamiento en dispositivos Android/iOS
        string fileName = "latency_data.csv";
        string filePath = Path.Combine(Application.persistentDataPath, fileName);

        // Verificar si el archivo existe, si no, escribir encabezados
        bool fileExists = File.Exists(filePath);

        using (StreamWriter writer = new StreamWriter(filePath, true))
        {
            if (!fileExists)
            {
                writer.WriteLine("T1,T2,T3,T4,T5,Points,Latency_Send,Latency_Transmit,Latency_Relay,Latency_Rebuild,Latency_Total");
            }
            writer.WriteLine($"{T1},{T2},{T3},{T4},{T5},{np},{latencySend},{latencyTransmit},{latencyRelay},{latencyRebuild},{latencyTotal}");
        }

        Debug.Log($"Datos de latencia guardados en: {filePath}");
    }
}
