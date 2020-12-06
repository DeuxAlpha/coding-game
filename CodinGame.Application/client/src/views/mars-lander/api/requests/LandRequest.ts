import {Map} from "@/views/mars-lander/models/environment/Map";
import {AiWeight} from "@/views/mars-lander/models/evolution/AiWeight";
import {EvolutionParameters} from "@/views/mars-lander/models/evolution/EvolutionParameters";

export class LandRequest {
  public Map!: Map
  public AiWeight!: AiWeight;
  public Parameters!: EvolutionParameters;

  constructor(init?: Partial<LandRequest>) {
    Object.assign(this, init);
  }
}