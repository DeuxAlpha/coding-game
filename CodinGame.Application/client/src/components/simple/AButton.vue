<template>
  <button class="mx-1 bg-blue-400 p-2 text-white">
    <slot></slot>
  </button>
</template>

<script lang="ts">
import {defineComponent, computed, SetupContext, ref, reactive, watch} from 'vue';

export default defineComponent({
  name: 'AButton',
})
</script>

<style scoped lang="scss">

</style>