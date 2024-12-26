﻿using System;
using System.Collections.Generic;

namespace Mako.Services.Shared
{
    public class ShiftsSelectQuery
    {
        public Guid IdCurrentShift { get; set; }
        public String Filter { get; set; }
    }

    public class ShiftsSelectDTO
    {
        public IEnumerable<Shift> Shifts { get; set; }
        public int Count { get; set; }

        public class Shift
        {
            public Guid Id { get; set; }
            public int Pier { get; set; }
            public DateOnly Date { get; set; }
            public TimeOnly StartHour { get; set; }
            public TimeOnly EndHour { get; set; }
            public ICollection<Worker> Workers { get; set; } = new List<Worker>();
            public string Ship {  get; set; }
        }
    }

    public class ShiftDetailQuery
    {
        public Guid Id { get; set; }
    }

    public class ShiftDetailDTO
    {
        public Guid Id { get; set; }
        public int Pier { get; set; }
        public DateOnly Date { get; set; }
        public TimeOnly StartHour { get; set; }
        public TimeOnly EndHour { get; set; }
        public string Workers { get; set; }
        public string Ship { get; set; }
    }
    }