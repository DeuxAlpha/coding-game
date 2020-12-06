import {GenerationActor} from "@/views/mars-lander/models/evolution/GenerationActor";

export class Generation {
  public GenerationNumber!: number;
  public Actors!: GenerationActor[];
}