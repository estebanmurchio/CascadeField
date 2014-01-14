CascadeField
============

Simple custom attribute implementation for creating a cascade field.

Usage
=====

The attribute has four parameters, the first one is the api url which returns the field text by the id sent, the second is a jQuery selector for the dropdown, the third is a jQuery selector for the input, and the last one is the name of the field to retrieve.

    public clas MyModel {
        [CascadeField("/api/demo/getfieldbyid", "select#DriverId", "input#VehiclePlate", "Plate")]
        public object PlateCascadeField { get; set; }
    }

API Example
===========

    // GET /api/driver/getfieldbyid/1?fieldName=Plate
    public string GetFieldById(long id, string fieldName)
    {
        if (id <= 0)
            throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest));

            var c = Uow.Drivers.Get(q => q.DriverId == id);

            if (c != null)
            {
                var t = c.GetType().GetProperty(fieldName).GetValue(c, null);
                return (string)t;
            }

        return string.Empty;
    }
