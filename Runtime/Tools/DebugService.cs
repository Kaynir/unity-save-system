using System;
using UnityEngine;

namespace Kaynir.Saves.Tools
{
    public static class DebugService
    {
        public static Logger Logger { get; private set; } = new Logger(Debug.unityLogger);

        public static void ThrowEmptyStringParseException()
        {
            throw new Exception("Unable to parse empty string.");
        }

        public static void LogJsonParseException(Exception exception)
        {
            Logger.Log($"Failed to parse JSON with exception: {exception}.");
        }

        public static void LogFileReadException(string filePath, Exception exception)
        {
            Logger.Log($"Failed to read {filePath} with exception: {exception}.");
        }

        public static void LogFileWriteException(string filePath, Exception exception)
        {
            Logger.Log($"Failed to write {filePath} with exception: {exception}.");
        }
    }
}