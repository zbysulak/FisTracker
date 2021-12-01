<template>
  <div>
    <v-text-field v-model="time"
      ><template v-slot:append>
        <v-dialog>
          <template v-slot:activator="{ on, attrs }">
            <v-btn color="primary" v-bind="attrs" v-on="on" icon>
              <v-icon>mdi-timer-outline</v-icon>
            </v-btn>
          </template>
          <template v-slot:default="dialog">
            <v-card>
              <v-card-text class="pa-0"
                ><v-time-picker v-model="time" format="24hr"></v-time-picker>
              </v-card-text>
              <v-card-actions class="justify-end">
                <v-btn text @click="dialog.value = false">Close</v-btn>
              </v-card-actions>
            </v-card>
          </template>
        </v-dialog>
      </template>
    </v-text-field>
  </div>
</template>
<script>
export default {
  props: ["time"],
  data: () => ({
    timepicker: false
  })
}
</script>
