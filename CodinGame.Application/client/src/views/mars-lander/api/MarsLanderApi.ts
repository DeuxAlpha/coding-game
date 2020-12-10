import axios from 'axios';
import {Map} from "@/views/mars-lander/models/environment/Map";
import {toPascal} from "@/helpers/PropertyTransformer";
import {Generation} from "@/views/mars-lander/models/evolution/Generation";
import {LandRequest} from "@/views/mars-lander/api/requests/LandRequest";
import {CalculateWeightRequest} from "@/views/mars-lander/api/requests/CalculateWeightRequest";

export class MarsLanderApi {
  private readonly apiClient = axios.create({
    baseURL: 'http://localhost:5000'
  })

  public async GetMaps(): Promise<Map[]> {
    return await this.apiClient.get("/mars-lander/maps")
      .then(response => {
        return toPascal(response.data) as Map[];
      })
  }

  public async Land(landRequest: LandRequest): Promise<Generation[]> {
    return await this.apiClient.post("mars-lander/land", landRequest)
      .then(response => {
        return toPascal(response.data) as Generation[];
      })
  }

  public async CalculateWeights(calculateWeightRequest: CalculateWeightRequest): Promise<void> {
    return await this.apiClient.post("mars-lander/calculate-weights", calculateWeightRequest)
      .then(response => {
        console.dir(response);
      });
  }
}