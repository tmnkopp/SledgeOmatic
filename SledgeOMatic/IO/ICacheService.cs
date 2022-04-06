namespace SOM
{
    public interface ICacheService{
        void Inspect();
        string Read();
        void Write(string content);
        void Append(string content); 
    }
}
