using AIrlineBookingSystem.Flights.Core.Entities;
using AIrlineBookingSystem.Flights.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Net.Http.Headers;
using System.Net.NetworkInformation;
using System.Text;
using Microsoft.Data.SqlClient;
using Dapper;
using System.Reflection.Metadata;

namespace AirlineBookingSystem.Flights.Infratsructure.Repositories
{
    public class FlightRepository : IFlightRepository
    {
        private readonly IDbConnection _dbConnection;

        public FlightRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task AddFlightAsync(Flight flight)
        {
            const string sql = @"Insert into Flights(Id,FlightNumber, Origin,Destination, DepartureTime, ArrivalTime)
                                Values(@Id,@FlightNumber, @Origin,@Destination, @DepartureTime, @ArrivalTime)";
            await _dbConnection.ExecuteAsync(sql, flight);
        }

        public async Task DeleteFlightAsync(Guid Id)
        {
            const string sql = @"DELETE FROM FLIGHTS WHERE ID = @Id";
            await _dbConnection.ExecuteAsync(sql, new { Id = Id });
        }

        public async Task<IEnumerable<Flight>> GetFlightsAsync()
        {
            const string sql = @"Select * from flights";
            return await _dbConnection.QueryAsync<Flight>(sql);
        }
    }
}
