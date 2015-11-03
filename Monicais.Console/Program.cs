
using Monicais.Core;
using Monicais.Property;
using System;

namespace Monicais.ConsoleTest
{

    internal class Program
    {

        private static void Main(string[] args)
        {
            ActionProcesses process = new ActionProcesses(new TimeMonoActionProcess(1,
                (e, t) =>
            {
                Console.WriteLine("Total excuted: " + t);
                return false;
            }));
            MonoAction action = new MonoAction("test", process);
        }

        private static void Main1(string[] args)
        {
            PropertyListManager manager2 = new PropertyManager().NewPropertyListManager("pl/role");
            PropertyID pid_hp = manager2.NewPropertyID("HealthPoint", "HP", "Health Points");
            PropertyID pid_hp_max = manager2.NewPropertyID("HealthPointMax", "Max HP", "Max Health Points");
            PropertyID pid_mp = manager2.NewPropertyID("ManaPoint", "MP", "Mana Points");
            PropertyID pid_mp_max = manager2.NewPropertyID("ManaPointMax", "Max MP", "Max Mana Points");
            PropertyID pid_vi = manager2.NewPropertyID("Vitality", "Vitality", "Vitality Points");
            PropertyID pid_vi_max = manager2.NewPropertyID("VitalityMax", "Max Vitality", "Max Vitality Points");
            SimplePropertyCreater creater = new SimplePropertyCreater();
            creater.AddAfterCreate(delegate (PropertyList pl)
            {
                EffectListenerUtil.SetOriginalFromFinalIfGreaterEqual(pl[pid_hp], pl[pid_hp_max]);
                EffectListenerUtil.SetOriginalFromFinalIfGreaterEqual(pl[pid_mp], pl[pid_mp_max]);
                EffectListenerUtil.SetOriginalFromFinalIfGreaterEqual(pl[pid_vi], pl[pid_vi_max]);
            });
            creater.setCreaterFor(pid_hp, SimplePropertyCreater.GetUnrecoverablePropertyCreater(100));
            creater.setCreaterFor(pid_hp_max, SimplePropertyCreater.GetRestorablePropertyCreater(100));
            creater.setCreaterFor(pid_mp, SimplePropertyCreater.GetUnrecoverablePropertyCreater(80));
            creater.setCreaterFor(pid_mp_max, SimplePropertyCreater.GetRestorablePropertyCreater(80));
            creater.setCreaterFor(pid_vi, SimplePropertyCreater.GetUnrecoverablePropertyCreater(100));
            creater.setCreaterFor(pid_vi_max, SimplePropertyCreater.GetRestorablePropertyCreater(100));
            creater.SetParentFor(pid_hp, 0, pid_hp_max);
            creater.SetParentFor(pid_mp, 0, pid_mp_max);
            creater.SetParentFor(pid_vi, 0, pid_vi_max);
            PropertyList list = manager2.CreateProperties(creater);
            foreach (var obj2 in list)
            {
                Console.WriteLine(obj2);
            }
            Console.WriteLine();
            Console.ReadKey(true);
            IProperty property = list[pid_hp];
            Console.WriteLine(property);
            property.AddEffect(new DefaultEffect(new EffectID("Reduce 10 hp", 0), pid_hp, null,
                (Attributes pa, Attributes ea, ref int v) => v -= 10, ea => false));
            Console.WriteLine(property);
            Console.WriteLine();
            Console.ReadKey(true);
            IProperty property2 = list[pid_hp_max];
            Console.WriteLine(property2);
            property2.AddEffect(new DefaultEffect(new EffectID("Reduce 50% max hp", 0), pid_hp_max, null,
                (Attributes pa, Attributes ea, ref int v) => v = v >> 1, ea => false));
            Console.WriteLine(property2);
            Console.WriteLine(property);
            Console.WriteLine();
            property2.RemoveEffect(new EffectID("Reduce 50% max hp", 0));
            Console.WriteLine(property2);
            Console.WriteLine(property);
            Console.ReadKey(true);
            Console.WriteLine("\nFinish");
            Console.ReadKey(true);
        }
    }
}
