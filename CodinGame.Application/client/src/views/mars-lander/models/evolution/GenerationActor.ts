import {Lander} from "@/views/mars-lander/models/evolution/Lander";

export class GenerationActor {
  public Score!: number;
  public Lander!: Lander;

  constructor(init?: Partial<GenerationActor>) {
    Object.assign(this, init);
  }
}