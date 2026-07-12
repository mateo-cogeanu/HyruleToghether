namespace BOTWM.Server.JSONBuilder
{

    public class JSONFormat<T>
    {

        private bool UsesLambda = false;
        public int Size;
        public string Function;
        public Func<T> Lambda;

        public JSONFormat(int size, string function = "", Func<T> lambda = null)
        {
            this.Size = size;
            
            if(function != null)
                Function = function;
            else
            {
                Lambda = lambda;
                UsesLambda = true;
            }

        }

        public T ToObject(byte[] data)
        {

            return (T)typeof(BitConverter).GetMethod(Function).Invoke(null, new[] {data});

        }

    }

}
