using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ots.Base;

public class SingletonService   // singleton servis asla yenilenmiyor 
{
    public int Counter;
}

public class ScopedService  // ayni class instanceı kullanılıyor aynı api call'da
{
    public int Counter;
}


public class TransientService  // her erismeye calistiginda yeni bir class instancei olusturuluyor
{
    public int Counter;
}