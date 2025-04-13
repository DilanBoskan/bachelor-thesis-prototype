using System.Numerics;

namespace Domain.Entities.Elements.InkStrokes;

public readonly record struct InkStrokePoint(Vector2 Position, float Pressure = 0.5f);
