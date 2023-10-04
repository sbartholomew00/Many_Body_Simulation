using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Text.Json;

namespace Many_Body_Simulation
{
    internal class SimulationManager
    {
        ISimulator simulator;
        bool paused;
        List<VisualBody> bodyList;
        //VisualBody? selectedBody;
        double timeStep;
        public SimulationManager(ISimulator simulator)
        {
            this.simulator = simulator;
            paused = true;
            bodyList = new();
            timeStep = 0.1;
            //selectedBody = null;
        }
        public void Update()
        {
            if (!paused)
                simulator.Tick<VisualBody>(bodyList, timeStep);
        }
        public void SetPaused(bool paused)
        {
            this.paused = paused;
        }
        public void LoadState(string jsonString)
        {
            try
            {
                List<VisualBody.VisualBodyState>? newState = JsonSerializer.Deserialize<List<VisualBody.VisualBodyState>>(jsonString);
                if (newState != null)
                {
                    bodyList.Clear();
                    foreach (VisualBody.VisualBodyState state in newState)
                    {
                        bodyList.Add(new VisualBody(state));
                    }
                }
            }
            catch { }
        }
        public string SaveState()
        {
            List<VisualBody.VisualBodyState> state = new();
            foreach(VisualBody body in bodyList)
            {
                state.Add(body.getState());
            }
            return JsonSerializer.Serialize(state);
        }
        public void AddBody(VisualBody body)
        {
            bodyList.Add(body);
        }
        public List<VisualBody> GetBodies()
        {
            return bodyList;
        }

        /*public void ChangeSelectedPosition(double xPosition)
        {
            if(selectedBody != null)
            {
                selectedBody.Position = new Tuple<double, double, double>(xPosition, selectedBody.Position.Item2, selectedBody.Position.Item3);
            }
        }*/

        /*public void SetSelectedBody(VisualBody body)
        {
            selectedBody = body;
        }*/
    }
}
