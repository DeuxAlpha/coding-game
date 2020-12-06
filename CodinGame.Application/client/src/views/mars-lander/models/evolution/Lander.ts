import {Situation} from "@/views/mars-lander/models/evolution/Situation";
import {LanderStatus} from "@/views/mars-lander/models/evolution/LanderStatus";

export class Lander {
  public Situation!: Situation;
  public Situations!: Situation[];
  public Actions!: string[];
  public Status!: LanderStatus;
}