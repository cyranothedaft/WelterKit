// using System;
// using System.Collections.Generic;
// using System.Text;
//
// namespace WelterKit.FunctionalContainers_tests.TransformerTests;
//
// [TestClass]
// internal class StateT_Tests {
//
//    private record Config(int FileHandle);
//    private record FileState(uint Position);
//
//    // -- 3. The combined Monad Stack
//    // type AppMonad a = ReaderT Config (StateT FileState IO) a
//    private record AppMonad<A>
//
//    [TestMethod]
//    public void xxx() {
//
//       // runApp :: Config -> FileState -> AppMonad a -> IO (a, FileState)
//       // runApp conf st app = runStateT (runReaderT app conf) st
//       (A value, int position) runApp<A>(Config config, FileState fileState, AppMonad<A> app) {
//
//       }
//
//       // import Control.Monad.Reader
//       // import Control.Monad.IO.Class
//       // 
//       // -- 1. Define the environment
//       // data Config = Config { dbConnection :: String, port :: Int }
//       // 
//       // -- 2. Define our app monad stack
//       // type App = ReaderT Config IO
//       // 
//       // -- 3. Function that uses the environment
//       // fetchData :: App ()
//       // fetchData = do
//       //     config <- ask  -- Retrieve environment
//       //     liftIO $ putStrLn $ "Connecting to: " ++ dbConnection config
//       //     liftIO $ putStrLn $ "Port: " ++ show (port config)
//       // 
//       // -- 4. Running the application
//       // main :: IO ()
//       // main = do
//       //     let myConfig = Config "postgres://localhost" 5432
//       //     runReaderT fetchData myConfig
//
//
//    }
// }
