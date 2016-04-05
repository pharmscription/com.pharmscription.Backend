﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.pharmscription.BusinessLogic.Patient;
using com.pharmscription.Infrastructure.Dto;

namespace com.pharmscription.Service.Tests
{
    internal class FakePatientManager: IPatientManager
    {
        private readonly List<PatientDto> _fakeDatabase;
        internal FakePatientManager()
        {
            _fakeDatabase = new List<PatientDto>
            {
                new PatientDto
                {
                    Id = "0",
                    FirstName = "Sara",
                    LastName = "Müller",
                    Address = GetAddress(0),
                    AhvNumber = "10",
                    BirthDate = new DateTime(1990, 12, 2),
                    Insurance = "Generika",
                    InsuranceNumber = "1",
                    PhoneNumber = "111"
                },
                new PatientDto
                {
                    Id = "1",
                    FirstName = "Livia",
                    LastName = "Danuser",
                    Address = GetAddress(1),
                    AhvNumber = "11",
                    BirthDate = new DateTime(1991, 3, 21),
                    Insurance = "Generika",
                    InsuranceNumber = "2",
                    PhoneNumber = "112"
                },
                new PatientDto
                {
                    Id = "2", 
                    FirstName = "Gianna",
                    LastName = "Clavadetscher",
                    Address = GetAddress(2),
                    AhvNumber = "12",
                    BirthDate = new DateTime(1992, 8, 27),
                    Insurance = "Generika",
                    InsuranceNumber = "3",
                    PhoneNumber = "113"
                }

            };
            
        }

        private AddressDto GetAddress(int index)
        {
            List<AddressDto> list = new List<AddressDto>
            {
                new AddressDto
                {
                    Id = "0",
                    Street = "Ringstrasse",
                    StreetExtension = "",
                    Number = "110",
                    City = "Chur",
                    CityCode = "7000"
                },
                new AddressDto
                {
                    Id = "1",
                    Street = "Nidaugasse",
                    StreetExtension = "2B",
                    Number = "17",
                    City = "Biel/Bienne",
                    CityCode = "2500"
                },
                new AddressDto
                {
                    Id = "2",
                    Street = "Alte Jonastrasse",
                    StreetExtension = "",
                    Number = "89",
                    City = "Rapperswil-Jona",
                    CityCode = "8640"
                }
            };
            return list[index];

        }
        public async Task<PatientDto> Lookup(string ahvNumber)
        {
            var patient = _fakeDatabase.FirstOrDefault(dto => dto.AhvNumber == ahvNumber);
            if (patient != null)
            {
                return patient;
            }
            throw new InvalidDataException();
        }

        public async Task<PatientDto> Add(PatientDto patient)
        {
            //TODO: handle invalid patient
            _fakeDatabase.Add(patient);
            return patient;
        }

        public async Task<PatientDto> Edit(PatientDto patient)
        {
            List<PatientDto> list = _fakeDatabase.Where(dto => dto.Id == patient.Id).ToList();
            if (list.Capacity == 0 || list.Capacity != 1)
            {
                throw new Exception("Id has not been found or has been found multiple times");
            }
            PatientDto p = list[0];
            p.FirstName = patient.FirstName;
            p.LastName = patient.LastName;
            p.Address = patient.Address;
            p.AhvNumber = patient.AhvNumber;
            p.BirthDate = patient.BirthDate;
            p.Insurance = patient.Insurance;
            p.InsuranceNumber = patient.InsuranceNumber;
            p.PhoneNumber = patient.PhoneNumber;
            return p;
        }

        public Task<PatientDto> Find(string ahvNumber)
        {
            return Lookup(ahvNumber);
        }

        public async Task<PatientDto> GetById(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<PatientDto> RemoveById(string id)
        {
            throw new NotImplementedException();
        }
    }
}
