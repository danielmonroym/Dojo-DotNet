  
using System;
using Google.Cloud.Firestore;
using System.Collections.Generic;
using System.Threading.Tasks;
using dojonet.Modelo;
namespace dojonet
{
    public class FirebaseAccount
    { 
        private readonly static FirebaseAccount _instance = new FirebaseAccount();
       FirestoreDb _db;
        public FirebaseAccount()
        {
             String path = AppDomain.CurrentDomain.BaseDirectory + @"Firebase-SDK.json";
             Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS",path);
            _db = FirestoreDb.Create("dojo-net-core");
            Console.WriteLine("sucessful");
            
        }
         public static FirebaseAccount Instance
        {
            get
            {
                return _instance;
            }
        }
        public async Task<String> AddUser(Usuario user){
            DocumentReference coll = _db.Collection("Usuarios").Document();
            Dictionary <String,Object> data= new Dictionary <String,Object>()
            {
                {"Nombre",user.Nombre},
                {"Identificación",user.Cedula},
                {"Correo",user.Correo},
                {"Carrera",user.Carrera}
            };
            await coll.SetAsync(data);
        
            return "Usuario guardado con Id: " + coll.Id;
        }
         public async Task<List<Usuario>> GetUser(){
            CollectionReference usuariosRef= _db.Collection("Usuarios");
            Query query =  usuariosRef;
            QuerySnapshot querySnapshot = await usuariosRef.GetSnapshotAsync();
            List<Usuario> ListaUsuarios = new List<Usuario>();
            foreach (DocumentSnapshot documentSnapshot in querySnapshot.Documents)
            {
                Usuario  usuario= new Usuario();
                Dictionary<String, Object> usuarios = documentSnapshot.ToDictionary();
                foreach (var item in usuarios)
                {
                    if(item.Key=="Nombre"){
                        usuario.Nombre=(String)item.Value;
                    }else if(item.Key=="Identificación"){
                        usuario.Cedula=(String)item.Value;
                    }else if(item.Key=="Correo"){
                        usuario.Correo=(String)item.Value;
                    }else if(item.Key=="Carrera"){
                        usuario.Carrera=(String)item.Value;
                    }
                }
                ListaUsuarios.Add(usuario);
            }
            return ListaUsuarios;
        }

        public async Task<String> deleteUser(String id){
            DocumentReference cityRef = _db.Collection("Usuarios").Document(id);
            await cityRef.DeleteAsync();
            return "Usuario con Id: "+id+" eliminado correctamente";
        }
    }


    }