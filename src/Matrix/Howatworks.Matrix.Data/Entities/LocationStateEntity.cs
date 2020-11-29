using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Howatworks.Matrix.Data.Entities
{
    public class LocationStateEntity : IMatrixEntity, IGameContextEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public DateTimeOffset TimeStamp { get; set; }
        public string GameVersion { get; set; }
        public string CommanderName { get; set; }

        public string Body_Name { get; set; }
        public string Body_Type { get; set; }
        public bool? Body_Docked { get; set; }

        public string SignalSource_Type_Symbol { get; set; }
        public string SignalSource_Type_Text { get; set; }
        public int? SignalSource_Threat { get; set; }

        public string StarSystem_Name { get; set; }
        public decimal StarSystem_Coords_X { get; set; }
        public decimal StarSystem_Coords_Y { get; set; }
        public decimal StarSystem_Coords_Z { get; set; }

        public string Station_Name { get; set; }
        public string Station_Type { get; set; }

        public bool? SurfaceLocation_Landed { get; set; }
        public decimal? SurfaceLocation_Longitude { get; set; }
        public decimal? SurfaceLocation_Latitude { get; set; }
    }
}
