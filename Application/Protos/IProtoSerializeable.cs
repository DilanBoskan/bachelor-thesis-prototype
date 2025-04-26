namespace Application.Protos;
public interface IProtoSerializeable<T, J> {
    abstract static T FromProto(J proto);
    abstract static J ToProto(T value);
}
