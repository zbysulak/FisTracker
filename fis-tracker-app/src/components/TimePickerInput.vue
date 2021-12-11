<template>
  <div>
    <v-menu
      ref="menu"
      v-model="picker"
      :close-on-content-click="false"
      :nudge-right="40"
      :return-value.sync="time"
      transition="scale-transition"
      offset-y
      max-width="290px"
      min-width="290px">
      <template v-slot:activator="{ on, attrs }">
        <v-text-field
          v-model="time"
          :label="label"
          prepend-icon="mdi-clock-time-four-outline"
          readonly
          v-bind="attrs"
          v-on="on"
          @click:clear="$emit('input', '')"
          :clearable="clearable"></v-text-field>
      </template>
      <v-time-picker
        v-if="picker"
        v-model="time"
        full-width
        format="24hr"
        v-on:input="$emit('input', time)"
        @click:minute="$refs.menu.save(time)"></v-time-picker>
    </v-menu>
  </div>
</template>
<script>
export default {
  props: {
    label: { type: String, default: "Time" },
    value: { type: String },
    clearable: { type: Boolean, default: false }
  },
  data: () => ({
    picker: false,
    time: ""
  }),
  watch: {
    value(val) {
      this.time = val
    }
  },
  mounted: function () {
    this.time = this.value
  }
}
</script>
