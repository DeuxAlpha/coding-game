<template>
  <div class="flex flex-col items-center">
    <div class="flex flex-row">
      <label for="selected-map">Map</label>
      <select id="selected-map" v-model="selectedMap" @change="onMapChange">
        <option v-for="mapElement of mapElements" :value="mapElement.Name">{{ mapElement.Name }}</option>
      </select>
    </div>
    <div class="w-full h-full" ref="map"/>
  </div>
</template>

<script lang="ts">
import {defineComponent, Ref, ref} from 'vue';
import {MarsLanderApi} from "@/api/MarsLanderApi";
import {Map} from "@/views/mars-lander/models/Map";
import * as Echarts from "echarts";

export default defineComponent({
  name: 'CMarsLander',
  async setup() {
    const map = ref(null) as Ref<null | HTMLDivElement>;
    const selectedMap = ref('');

    const marsLanderApi = new MarsLanderApi();

    const graph = ref(null) as Ref<null | Echarts.Chart>;

    const mapElements = await marsLanderApi.GetMaps();

    function onMapChange() {
      const selectedMapModel = mapElements.find(element => element.Name === selectedMap.value);

      renderMap(graph, map.value as HTMLDivElement, selectedMapModel as Map);
    }

    function renderMap(graph: Ref<null | Echarts.Chart>, map: HTMLDivElement, mapElement: Map) {
      if (graph.value === null) {
        graph.value = Echarts.init(map);
      }
      const surfaceArray = mapElement.SurfaceElements.map(element => [element.X, element.Y]);
      console.dir(surfaceArray);
      const chartOptions = {
        title: {
          text: mapElement.Name
        },
        xAxis: {},
        yAxis: {},
        series: {
          type: 'line',
          data: surfaceArray
        }
      } as Echarts.EChartsOption;
      graph.value.setOption(chartOptions);
    }

    return {mapElements, selectedMap, onMapChange, map, renderMap};
  }
})
</script>

<style scoped lang="scss">

</style>