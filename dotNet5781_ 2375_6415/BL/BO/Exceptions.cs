using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    [Serializable]
    public class BOReadDataException : Exception
    {
        public BOReadDataException() : base() { }
        public BOReadDataException(string message) : base(message) { }
        public BOReadDataException(string message, Exception inner) : base(message, inner) { }
        protected BOReadDataException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }

        [Serializable]
    public class BOBadBusException : Exception
    {
        public int iD;
        public BOBadBusException(int id) : base() => iD = id;
        public BOBadBusException(string message, int id) : base(message) => iD = id;
        public BOBadBusException(string message, Exception inner, int id) : base(message, inner) => iD = id;
        protected BOBadBusException(SerializationInfo info, StreamingContext context, int id) : base(info, context) => iD = id;
        public override string ToString() => base.ToString() + $", bad Bus id: {iD}";
    }

    [Serializable]
    public class BOBadLineException : Exception
    {
        public int iD;
        public BOBadLineException(int id) : base() => iD = id;
        public BOBadLineException(string message, int id) : base(message) => iD = id;
        public BOBadLineException(string message, Exception inner, int id) : base(message, inner) => iD = id;
        protected BOBadLineException(SerializationInfo info, StreamingContext context, int id) : base(info, context) => iD = id;
        public override string ToString() => base.ToString() + $", bad Line id: {iD}";
    }

    [Serializable]
    public class BOBadStationException : Exception
    {
        public int iD;
        public BOBadStationException(int id) : base() => iD = id;
        public BOBadStationException(string message, int id) : base(message) => iD = id;
        public BOBadStationException(string message, Exception inner, int id) : base(message, inner) => iD = id;
        protected BOBadStationException(SerializationInfo info, StreamingContext context, int id) : base(info, context) => iD = id;
        public override string ToString() => base.ToString() + $", bad Station id: {iD}";
    }

    [Serializable]
    public class BOBadUserException : Exception
    {
        public string userName;
        public BOBadUserException(string user) : base() => userName = user;
        public BOBadUserException(string message, string user) : base(message) => userName = user;
        public BOBadUserException(string message, Exception inner, string user) : base(message, inner) => userName = user;
        protected BOBadUserException(SerializationInfo info, StreamingContext context, string user) : base(info, context) => userName = user;
        public override string ToString() => base.ToString() + $", bad User name {userName}";
    }

    public class BOBadLineStationException : Exception
    {
        public int line;
        public int station;
        public BOBadLineStationException(int tmpLine, int tmpStation) : base() {line = tmpLine; station = tmpStation;}

        public BOBadLineStationException(string message, int tmpLine, int tmpStation) : base(message) { line = tmpLine; station = tmpStation; }
        public BOBadLineStationException(string message, Exception inner, int tmpLine, int tmpStation) : base(message, inner) { line = tmpLine; station = tmpStation; }
        protected BOBadLineStationException(SerializationInfo info, StreamingContext context, int tmpLine, int tmpStation) : base(info, context) { line = tmpLine; station = tmpStation; }
        public override string ToString() => base.ToString() + $", bad LineStation : {"/r"} line :{line}, station:{station}" ;
    }

    [Serializable]
    public class BOBadUserTripException : Exception
    {
        public string userName;
        public BOBadUserTripException(string user) : base() => userName = user;
        public BOBadUserTripException(string message, string user) : base(message) => userName = user;
        public BOBadUserTripException(string message, Exception inner, string user) : base(message, inner) => userName = user;
        protected BOBadUserTripException(SerializationInfo info, StreamingContext context, string user) : base(info, context) => userName = user;
        public override string ToString() => base.ToString() + $", bad UserTrip name {userName}";
    }

    public class BOBadPairStationException : Exception
    {
        public int firstStation;
        public int lastStation;
        public BOBadPairStationException(int tmpFirstStation, int tmpLastStation) : base() { firstStation = tmpFirstStation; lastStation = tmpLastStation; }

        public BOBadPairStationException(string message, int tmpFirstStation, int tmpLastStation) : base(message) { firstStation = tmpFirstStation; lastStation = tmpLastStation; }
        public BOBadPairStationException(string message, Exception inner, int tmpFirstStation, int tmpLastStation) : base(message, inner) { firstStation = tmpFirstStation; lastStation = tmpLastStation; }
        protected BOBadPairStationException(SerializationInfo info, StreamingContext context, int tmpFirstStation, int tmpLastStation) : base(info, context) { firstStation = tmpFirstStation; lastStation = tmpLastStation; }
        public override string ToString() => base.ToString() + $", bad PairStation : {"/r"} firstStation :{firstStation}, lastStation:{lastStation}";
    }

    [Serializable]
    public class BOBadBusInTravelException : Exception
    {
        public int license;
        public int lineNumber;
        public DateTime departureTime;
        public BOBadBusInTravelException(int tmpLicense, int tmpLineNumber, DateTime tmpDepartureTime) : base() { license = tmpLicense; lineNumber = tmpLineNumber; departureTime = tmpDepartureTime; }
        public BOBadBusInTravelException(string message, int tmpLicense, int tmpLineNumber, DateTime tmpDepartureTime) : base(message) { license = tmpLicense; lineNumber = tmpLineNumber; departureTime = tmpDepartureTime; }
        public BOBadBusInTravelException(string message, Exception inner, int tmpLicense, int tmpLineNumber, DateTime tmpDepartureTime) : base(message, inner) { license = tmpLicense; lineNumber = tmpLineNumber; departureTime = tmpDepartureTime; }
        protected BOBadBusInTravelException(SerializationInfo info, StreamingContext context, int tmpLicense, int tmpLineNumber, DateTime tmpDepartureTime) : base(info, context) { license = tmpLicense; lineNumber = tmpLineNumber; departureTime = tmpDepartureTime; }
        public override string ToString() => base.ToString() + $", bad BusInTravel: license = {license}, lineNumber = {lineNumber}, departureTime = {departureTime}";
    }
    public class BOBadLineDepartingException : Exception
    {
        public int lineNumber;
        public DateTime startTime;
        public BOBadLineDepartingException(int tmpLineNumber, DateTime tmpStartTime) : base() {lineNumber = tmpLineNumber; startTime = tmpStartTime; }
        public BOBadLineDepartingException(string message, int tmpLineNumber, DateTime tmpStartTime) : base(message) { lineNumber = tmpLineNumber; startTime = tmpStartTime; }
        public BOBadLineDepartingException(string message, Exception inner, int tmpLineNumber, DateTime tmpStartTime) : base(message, inner) { lineNumber = tmpLineNumber; startTime = tmpStartTime; }
        protected BOBadLineDepartingException(SerializationInfo info, StreamingContext context, int tmpLineNumber, DateTime tmpStartTime) : base(info, context) { lineNumber = tmpLineNumber; startTime = tmpStartTime; }
        public override string ToString() => base.ToString() + $", bad LineDeparting: lineNumber = {lineNumber}, startTime = {startTime}";
    }
}
