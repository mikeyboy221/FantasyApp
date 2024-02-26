using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Collections;
using Newtonsoft.Json.Linq;

using FantasyApp.Models;
using System.ComponentModel;
using Newtonsoft.Json.Converters;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Runtime.CompilerServices;
using System.Text.Json;

namespace FantasyApp.Services;

public class PointsService : IPointsService
{
    public PointsService()
    {

    }

    public int GetScore()
    {
        return 0;
    }
}

public interface IPointsService
{
    public int GetScore();
}