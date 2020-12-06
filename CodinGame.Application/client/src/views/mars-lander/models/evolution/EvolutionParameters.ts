export class EvolutionParameters {
  public Generations!: number;
  public Population!: number;

  constructor(init?: Partial<EvolutionParameters>) {
    Object.assign(this, init);
  }
}