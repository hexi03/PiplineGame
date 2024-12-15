using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
    public class PathSquarer {
    // Выравнивание точек пути по заданному шагу (тайлам)
    public static List<Vector3> SquarePath(List<Vector3> path, Vector3 step)
    {
        List<Vector3> squaredPath = new List<Vector3>();

        for (int i = 0; i < path.Count; i++)
        {
            // Шаг для выравнивания по тайлам (округляем координаты к ближайшим шагам)
            Vector3 alignedPoint = AlignToStep(path[i], step);
            //squaredPath.Add(alignedPoint); //смещение на пол клетки для получения центра
            // Добавляем первую выровненную точку
            if (i == 0)
            {
                squaredPath.Add(alignedPoint + step /2);
            }
            else
            {
                // Предыдущая точка
                Vector3 lastPoint = squaredPath[squaredPath.Count - 1];

                // Проверяем, идет ли текущий сегмент по диагонали
                if (IsDiagonal(lastPoint, alignedPoint))
                {
                    // Если сегмент диагональный, добавляем промежуточную точку
                    AddIntermediatePoints(squaredPath, lastPoint, alignedPoint, step);
                }

                // Добавляем текущую точку в любом случае
                squaredPath.Add(alignedPoint + step /2);
            }
        }

        return squaredPath;
    }

    // Выровнять координаты точки к ближайшим тайлам
    private static Vector3 AlignToStep(Vector3 point, Vector3 step)
    {
        float x = Mathf.Round(point.x / step.x) * step.x;
        float y = Mathf.Round(point.y / step.y) * step.y;
        float z = step.z;

        return new Vector3(x, y, z);
    }

    // Проверка, идет ли отрезок по диагонали
    private static bool IsDiagonal(Vector3 pointA, Vector3 pointB)
    {
        return pointA.x != pointB.x && pointA.z != pointB.z;
    }

    // Добавляем промежуточную точку, чтобы создать прямой угол
    private static void AddIntermediatePoints(List<Vector3> path, Vector3 from, Vector3 to, Vector3 step)
    {
        // Для корректного добавления "лесенки" между двумя диагональными точками
        if (Mathf.Abs(to.x - from.x) > Mathf.Abs(to.z - from.z))
        {
            // Двигаемся сначала по оси X, потом по оси Z
            path.Add(new Vector3(to.x, from.y, from.z));
        }
        else
        {
            // Двигаемся сначала по оси Z, потом по оси X
            path.Add(new Vector3(from.x, from.y, to.z));
        }
    }
}
}