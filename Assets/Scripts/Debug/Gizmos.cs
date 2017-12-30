﻿//by popcron.itch.io

using UnityEngine;
using System.Collections.Generic;

public class GizmosHelper
{
    public static void DrawLine(Vector3 a, Vector3 b, Color color)
    {
        if (!GizmoManager.Show) return;

        GizmoManager.lines.Add(new GizmoManager.GizmoLine(a, b, color));

        try
        {
            UnityEngine.Gizmos.color = color;
            UnityEngine.Gizmos.DrawLine(a, b);
        }
        catch
        {
            //ignores the annoying
            //UnityException: Gizmo drawing functions can only be used in OnDrawGizmos and OnDrawGizmosSelected.
            //error
        }
    }

    public static void DrawBox(Vector3 position, Vector3 size, Color color)
    {
        if (!GizmoManager.Show) return;

        Vector3 point1 = new Vector3(position.x - size.x * 0.5f, position.y - size.y * 0.5f, position.z - size.z * 0.5f);
        Vector3 point2 = new Vector3(position.x + size.x * 0.5f, position.y - size.y * 0.5f, position.z - size.z * 0.5f);
        Vector3 point3 = new Vector3(position.x + size.x * 0.5f, position.y + size.y * 0.5f, position.z - size.z * 0.5f);
        Vector3 point4 = new Vector3(position.x - size.x * 0.5f, position.y + size.y * 0.5f, position.z - size.z * 0.5f);

        Vector3 point5 = new Vector3(position.x - size.x * 0.5f, position.y - size.y * 0.5f, position.z + size.z * 0.5f);
        Vector3 point6 = new Vector3(position.x + size.x * 0.5f, position.y - size.y * 0.5f, position.z + size.z * 0.5f);
        Vector3 point7 = new Vector3(position.x + size.x * 0.5f, position.y + size.y * 0.5f, position.z + size.z * 0.5f);
        Vector3 point8 = new Vector3(position.x - size.x * 0.5f, position.y + size.y * 0.5f, position.z + size.z * 0.5f);

        DrawLine(point1, point2, color);
        DrawLine(point2, point3, color);
        DrawLine(point3, point4, color);
        DrawLine(point4, point1, color);

        DrawLine(point5, point6, color);
        DrawLine(point6, point7, color);
        DrawLine(point7, point8, color);
        DrawLine(point8, point5, color);

        DrawLine(point1, point5, color);
        DrawLine(point2, point6, color);
        DrawLine(point3, point7, color);
        DrawLine(point4, point8, color);
    }

    public static void DrawSquare(Vector3 position, Vector3 size, Color color)
    {
        if (!GizmoManager.Show) return;

        Vector3 point1 = new Vector3(position.x - size.x * 0.5f, position.y - size.y * 0.5f, position.z);
        Vector3 point2 = new Vector3(position.x + size.x * 0.5f, position.y - size.y * 0.5f, position.z);
        Vector3 point3 = new Vector3(position.x + size.x * 0.5f, position.y + size.y * 0.5f, position.z);
        Vector3 point4 = new Vector3(position.x - size.x * 0.5f, position.y + size.y * 0.5f, position.z);

        DrawLine(point1, point2, color);
        DrawLine(point2, point3, color);
        DrawLine(point3, point4, color);
        DrawLine(point4, point1, color);
    }

    public static void DrawCircle(Vector3 position, float radius, Color color)
    {
        if (!GizmoManager.Show) return;

        DrawPolygon(position, radius, 18, color);
    }

    public static void DrawPolygon(Vector3 position, float radius, int points, Color color)
    {
        if (!GizmoManager.Show) return;

        float angle = 360f / points;

        for (int i = 0; i < points; ++i)
        {
            float sx = Mathf.Cos(Mathf.Deg2Rad * angle * i) * radius * 0.5f;
            float sy = Mathf.Sin(Mathf.Deg2Rad * angle * i) * radius * 0.5f;

            float nx = Mathf.Cos(Mathf.Deg2Rad * angle * (i + 1)) * radius * 0.5f;
            float ny = Mathf.Sin(Mathf.Deg2Rad * angle * (i + 1)) * radius * 0.5f;

            Vector3 a = new Vector3(sx, sy, position.z);
            Vector3 b = new Vector3(nx, ny, position.z);

            DrawLine(position + a, position + b, color);
        }
    }
}