import {EvolutionParameters} from "@/views/mars-lander/models/evolution/EvolutionParameters";
import {AiWeight} from "@/views/mars-lander/models/evolution/AiWeight";
import {Map} from "@/views/mars-lander/models/environment/Map";

export class CalculateWeightRequest {
  public Parameters!: EvolutionParameters;
  public Map!: Map;
  public OriginalWeights!: AiWeight;
  public ChangePerTry!: number;
  public UpwardTries!: number;
  public DownwardTries!: number;

  constructor(init?: Partial<CalculateWeightRequest>) {
    Object.assign(this, init);
  }
}